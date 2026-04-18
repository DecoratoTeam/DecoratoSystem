#!/usr/bin/env python3
"""
Download 150 interior design images from HuggingFace dataset kanbaa/InteriorDesigns
Saves images to: Persistence/DecorteeSystem/wwwroot/static/
Generates:       Persistence/Infrastructure/SeederData.json  (used by DbSeeder)

Usage:
  python download_images.py          -> downloads 150 images
  python download_images.py 200      -> downloads 200 images
"""

import os, sys, json, urllib.request, time
from pathlib import Path

DATASET    = "kanbaa/InteriorDesigns"
API_URL    = f"https://datasets-server.huggingface.co/rows?dataset={DATASET}&config=default&split=train"
OUT_DIR    = r"c:\Users\Admin\Desktop\DecoratoSystem\Persistence\DecorteeSystem\wwwroot\static"
SEEDER_OUT = r"c:\Users\Admin\Desktop\DecoratoSystem\Persistence\Infrastructure\SeederData.json"
TOTAL      = int(sys.argv[1]) if len(sys.argv) > 1 else 150

# ── keyword → category maps ───────────────────────────────────────────────────
ROOM_KEYWORDS = {
    "Living Room": ["living room","lounge","sitting room","couch","sofa","fireplace",
                    "tv ","television","sectional","coffee table","armchair"],
    "Bedroom":     ["bedroom","bed ","headboard","pillow","sleeping","mattress",
                    "duvet","nightstand","bed with"],
    "Kitchen":     ["kitchen","counter","cabinet","stove","oven","sink","island",
                    "marble counter","cooking","refrigerator","backsplash"],
    "Bathroom":    ["bathroom","bath","shower","toilet","vanity","tile","faucet","tub"],
    "Office":      ["office","desk","workspace","study","work from home",
                    "bookshelf","bookcase","computer"],
    "Dining Room": ["dining","dining room","dining table","dining area","dinner table",
                    "dining chair"],
}

STYLE_KEYWORDS = {
    "Modern":       ["modern","contemporary","sleek","clean lines","minimalist modern",
                     "large window","smart","high-tech"],
    "Minimalist":   ["minimalist","minimal","simple","uncluttered","bare","white wall",
                     "white couch","neutral","monochrome"],
    "Rustic":       ["rustic","wood","wooden","farmhouse","barn","natural","earthy",
                     "cozy","log","stone","brick wall"],
    "Industrial":   ["industrial","exposed brick","metal","loft","concrete","urban",
                     "pipe","raw","factory"],
    "Scandinavian": ["scandinavian","scandi","nordic","hygge","light wood","beige",
                     "white couch","birch","pine"],
    "Traditional":  ["traditional","classic","elegant","chandel","chandelier","ornate",
                     "luxury","marble floor","gold","antique","royal"],
}

POPULAR_WORDS  = ["luxury","elegant","chic","marble","chandel","large window",
                  "gold","ornate","high-end","spacious","stunning"]
TRENDING_WORDS = ["modern","minimalist","scandinavian","large bed","large island",
                  "sleek","contemporary","smart"]

def classify(text: str):
    t = text.lower()
    room  = next((r for r, kws in ROOM_KEYWORDS.items() if any(k in t for k in kws)), "Living Room")
    style = next((s for s, kws in STYLE_KEYWORDS.items() if any(k in t for k in kws)), "Modern")
    popular  = any(w in t for w in POPULAR_WORDS)
    trending = any(w in t for w in TRENDING_WORDS)
    # rating based on content quality signals
    h = abs(hash(text)) % 100
    if any(w in t for w in ["luxury","elegant","marble","chandel","gold","ornate"]):
        rating = round(min(10.0, 8.0 + h / 50), 1)
    elif any(w in t for w in ["modern","minimalist","scandinavian","sleek"]):
        rating = round(min(10.0, 7.0 + h / 40), 1)
    else:
        rating = round(min(10.0, 6.0 + h / 35), 1)
    views = 200 + abs(hash(text + "v")) % 9800
    return room, style, popular, trending, rating, views

def fetch_metadata(total: int):
    rows, batch = [], 100
    for offset in range(0, total, batch):
        length = min(batch, total - offset)
        url = f"{API_URL}&offset={offset}&length={length}"
        try:
            req = urllib.request.Request(url, headers={"User-Agent": "Mozilla/5.0"})
            with urllib.request.urlopen(req, timeout=20) as r:
                data = json.loads(r.read().decode())
            chunk = data.get("rows", [])
            rows.extend(chunk)
            print(f"  Fetched metadata rows {offset+1}–{offset+len(chunk)}")
        except Exception as e:
            print(f"  ERROR fetching offset {offset}: {e}")
            break
        time.sleep(0.1)
    return rows[:total]

def download_image(src: str, dest: str) -> bool:
    req = urllib.request.Request(src, headers={"User-Agent": "Mozilla/5.0"})
    with urllib.request.urlopen(req, timeout=30) as r:
        data = r.read()
    if len(data) < 1000:
        return False
    with open(dest, "wb") as f:
        f.write(data)
    return True

def main():
    Path(OUT_DIR).mkdir(parents=True, exist_ok=True)
    print(f"\n{'='*65}")
    print(f" Downloading {TOTAL} interior design images from HuggingFace")
    print(f"{'='*65}\n")

    print("Step 1: Fetching metadata...\n")
    rows = fetch_metadata(TOTAL)
    print(f"\n  Total rows received: {len(rows)}\n")

    if not rows:
        print("ERROR: No rows fetched. Check your internet connection.")
        return

    print(f"Step 2: Downloading images to:\n  {OUT_DIR}\n")
    print(f"{'─'*65}")

    metadata, ok, skip, fail = [], 0, 0, 0

    for row in rows:
        i    = row["row_idx"]
        text = row["row"]["text"]
        src  = row["row"]["image"]["src"]
        ext  = "png" if "image.png" in src else "jpg"
        fname = f"interior-{i}.{ext}"
        fpath = os.path.join(OUT_DIR, fname)
        room, style, popular, trending, rating, views = classify(text)

        if os.path.exists(fpath) and os.path.getsize(fpath) > 1000:
            print(f"[{i:3d}] SKIP  {fname:25s}| {room:12s}| {style}")
            skip += 1
            ok   += 1
        else:
            print(f"[{i:3d}] DOWN  {fname:25s}| {room:12s}| {style} ... ", end="", flush=True)
            try:
                if download_image(src, fpath):
                    size = os.path.getsize(fpath) // 1024
                    print(f"OK ({size}KB)")
                    ok += 1
                else:
                    print("FAIL (too small)")
                    fail += 1
            except Exception as e:
                print(f"FAIL ({str(e)[:35]})")
                fail += 1
            time.sleep(0.05)

        metadata.append({
            "index":       i,
            "file":        f"/static/{fname}",
            "description": text,
            "room":        room,
            "style":       style,
            "popular":     popular,
            "trending":    trending,
            "rating":      rating,
            "views":       views
        })

    # ── write SeederData.json ─────────────────────────────────────────────────
    with open(SEEDER_OUT, "w", encoding="utf-8") as f:
        json.dump(metadata, f, indent=2, ensure_ascii=False)

    print(f"\n{'='*65}")
    print(f"  Downloaded : {ok - skip} new images")
    print(f"  Skipped    : {skip} (already exist)")
    print(f"  Failed     : {fail}")
    print(f"  Total OK   : {ok}/{len(rows)}")
    print(f"\n  SeederData.json → {SEEDER_OUT}")
    print(f"{'='*65}")
    print(f"\nNext step: run the server ->")
    print(f"  dotnet run --project Persistence\\DecorteeSystem\n")

if __name__ == "__main__":
    main()

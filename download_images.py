#!/usr/bin/env python3
"""
Download interior design images from HuggingFace dataset kanbaa/InteriorDesigns
and generate SeederData.json with smart room type, style, rating, and popularity.
Usage: python download_images.py [count]   (default: 100)
"""

import os, sys, json, urllib.request, time
from pathlib import Path

DATASET    = "kanbaa/InteriorDesigns"
API_URL    = f"https://datasets-server.huggingface.co/rows?dataset={DATASET}&config=default&split=train"
OUT_DIR    = r"c:\Users\Admin\Desktop\DecoratoSystem\Persistence\DecorteeSystem\wwwroot\static"
SEEDER_OUT = r"c:\Users\Admin\Desktop\DecoratoSystem\Persistence\Infrastructure\SeederData.json"
TOTAL      = int(sys.argv[1]) if len(sys.argv) > 1 else 100

# ── keyword maps ──────────────────────────────────────────────────────────────
ROOM_KEYWORDS = {
    "Living Room": ["living room","lounge","sitting room","couch","sofa","fireplace","tv ","television","sectional"],
    "Bedroom":     ["bedroom","bed ","headboard","pillow","sleeping","mattress","duvet"],
    "Kitchen":     ["kitchen","counter","cabinet","stove","oven","sink","island","marble counter","cooking"],
    "Bathroom":    ["bathroom","bath","shower","toilet","vanity","tile","faucet"],
    "Office":      ["office","desk","workspace","study","work from home","bookshelf","bookcase"],
    "Dining Room": ["dining","dining room","dining table","dining area","dinner table"],
}

STYLE_KEYWORDS = {
    "Modern":       ["modern","contemporary","sleek","clean lines"],
    "Minimalist":   ["minimalist","minimal","simple","uncluttered","bare","white walls"],
    "Rustic":       ["rustic","wood","wooden","farmhouse","barn","natural","earthy","cozy"],
    "Industrial":   ["industrial","exposed brick","metal","loft","concrete","urban","pipe"],
    "Scandinavian": ["scandinavian","scandi","nordic","hygge","light wood","beige","white couch"],
    "Traditional":  ["traditional","classic","elegant","chandel","chandelier","ornate","luxury","marble floor"],
}

POPULAR_SIGNALS  = ["luxury","elegant","chic","marble","chandel","modern living","large window","white couch","gold"]
TRENDING_SIGNALS = ["modern","minimalist","scandinavian","large bed","large island","sleek"]

def classify(text):
    t = text.lower()
    room  = next((r for r, kws in ROOM_KEYWORDS.items() if any(k in t for k in kws)), "Living Room")
    style = next((s for s, kws in STYLE_KEYWORDS.items() if any(k in t for k in kws)), "Modern")
    popular  = any(s in t for s in POPULAR_SIGNALS)
    trending = any(s in t for s in TRENDING_SIGNALS)
    if any(s in t for s in ["luxury","elegant","marble","chandel","gold"]):
        rating = round(min(10.0, 8.5 + (hash(text) % 15) / 10), 1)
    elif "modern" in t or "minimalist" in t:
        rating = round(min(10.0, 7.5 + (hash(text) % 15) / 10), 1)
    else:
        rating = round(min(10.0, 6.5 + (hash(text) % 20) / 10), 1)
    views = 100 + abs(hash(text)) % 9900
    return room, style, popular, trending, rating, views

def fetch_rows(total):
    rows, batch = [], 100
    for offset in range(0, total, batch):
        url = f"{API_URL}&offset={offset}&length={min(batch, total - offset)}"
        try:
            with urllib.request.urlopen(url, timeout=15) as r:
                data = json.loads(r.read().decode())
            rows.extend(data.get("rows", []))
            print(f"  Fetched metadata {offset+1}-{min(offset+batch, total)}")
        except Exception as e:
            print(f"  ERROR at offset {offset}: {e}")
            break
    return rows[:total]

def main():
    Path(OUT_DIR).mkdir(parents=True, exist_ok=True)
    print(f"\nFetching metadata for {TOTAL} images...\n")
    rows = fetch_rows(TOTAL)
    print(f"\nGot {len(rows)} rows. Downloading...\n" + "-"*65)

    metadata, ok = [], 0
    for row in rows:
        i       = row["row_idx"]
        text    = row["row"]["text"]
        src     = row["row"]["image"]["src"]
        ext     = "png" if "image.png" in src else "jpg"
        fname   = f"interior-{i}.{ext}"
        fpath   = os.path.join(OUT_DIR, fname)
        room, style, popular, trending, rating, views = classify(text)

        if os.path.exists(fpath) and os.path.getsize(fpath) > 0:
            print(f"[{i:3d}] skip  {fname:22s} | {room:12s} | {style}")
            ok += 1
        else:
            try:
                print(f"[{i:3d}] down  {fname:22s} | {room:12s} | {style} ... ", end="", flush=True)
                req = urllib.request.Request(src, headers={"User-Agent": "Mozilla/5.0"})
                with urllib.request.urlopen(req, timeout=30) as r:
                    with open(fpath, "wb") as f: f.write(r.read())
                print(f"OK {os.path.getsize(fpath)//1024}KB")
                ok += 1
            except Exception as e:
                print(f"FAIL {str(e)[:40]}")

        metadata.append({
            "index": i, "file": f"/static/{fname}",
            "description": text, "room": room, "style": style,
            "popular": popular, "trending": trending,
            "rating": rating, "views": views
        })
        time.sleep(0.05)

    with open(SEEDER_OUT, "w", encoding="utf-8") as f:
        json.dump(metadata, f, indent=2, ensure_ascii=False)

    print(f"\n{'='*65}")
    print(f"Downloaded : {ok}/{len(rows)}")
    print(f"Metadata   : {SEEDER_OUT}")
    print(f"{'='*65}\n")

if __name__ == "__main__":
    main()

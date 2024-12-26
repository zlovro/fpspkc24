import math
from PIL import Image, ImageDraw

vertices_per_chunk = 256
chunk_area_vtx = vertices_per_chunk * vertices_per_chunk

render_distance = 20000 // vertices_per_chunk
chunk_data_fmt = 2
chunk_range = 2 * render_distance + 1
distance = chunk_range * 256

print(render_distance, 'render distance')
print(distance / 1000, 'km')
print(f'{chunk_range * chunk_range:,}', 'chunks')


def sizeof_fmt(num, suffix="B"):
    for unit in ("", "Ki", "Mi", "Gi", "Ti", "Pi", "Ei", "Zi"):
        if abs(num) < 1024.0:
            return f"{num:3.1f}{unit}{suffix}"
        num /= 1024.0
    return f"{num:.1f}Yi{suffix}"


def lod(x1, y1, x2, y2):
    d = math.dist((x1, y1), (x2, y2))
    if d < 1:
        return 0

    # ret = d
    # ret = d - 1
    # ret = d / 1.6
    ret = math.log(d, 1.5)
    return min(max(int(ret), 0), int(math.log2(vertices_per_chunk)) - 1)


raster = Image.new('RGB', (chunk_range * 8, chunk_range * 8))
drawer = ImageDraw.ImageDraw(raster)

tris = 0
size = 0
for y in range(chunk_range):
    for x in range(chunk_range):
        level_of_detail = lod(x, y, render_distance, render_distance)
        area = chunk_area_vtx >> level_of_detail
        size += area * chunk_data_fmt
        lod_dim = (vertices_per_chunk >> level_of_detail) - 1
        tris += lod_dim * lod_dim * 6

        c = level_of_detail * 15
        drawer.rectangle([(x * 8, y * 8), ((x + 1) * 8, (y + 1) * 8)], (c, 0, 0))

        line_color = (20, 20, 20)
        drawer.line([(x * 8, y * 8), ((x + 1) * 8, (y + 1) * 8)], line_color, 1)
        drawer.line([((x + 1) * 8, y * 8), (x * 8, (y + 1) * 8)], line_color, 1)

print(f'{tris:,}', 'tris')
raster.save('lod.png')

print(sizeof_fmt(size))

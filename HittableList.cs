using System.Security.AccessControl;
using Point3 = RayTracing.Vec3;

namespace RayTracing;

internal class HittableList : Hittable
{
    public List<Hittable> HitObjects = [];

    public HittableList()
    {
    }

    public HittableList(Hittable obj)
    {
        HitObjects.Add(obj);
    }

    public void Add(Hittable obj)
    { 
        HitObjects.Add(obj);
    }
    public void Clear()
    {
        HitObjects.Clear();
    }

    public override bool Hit(Ray r, Interval ray_t, ref HitRecord rec)
    {
        var temp_rec = new HitRecord();
        bool hit_anything = false;
        var closest_so_far = ray_t.max;

        foreach (Hittable obj in HitObjects)
        {
            if(obj.Hit(r, new Interval(ray_t.min, closest_so_far), ref temp_rec))
            {
                hit_anything = true;
                closest_so_far = temp_rec.t;
                rec = temp_rec;
            }
        }

        return hit_anything;
    }
}

namespace RaytracerNet
{
    using System;
    using System.Collections.Generic;
    using System.Numerics;

    public class KdTree
    {
        public readonly KdNode Root;
        private const int MaxDepth = 100;
        private const int Threshold = 3;

        public KdTree(List<Trig> triangles)
        {
            Root = new KdNode(triangles, MaxDepth, Threshold);
        }
    }

    public class KdNode
    {
        private KdNode left, right, middle;
        private int dimSplit;
        private bool isLeaf;
        private List<Trig> trigs; // only for leaves
        private readonly BoundingBox box = new BoundingBox();

        private const float Epsilon = 0.00001f;

        public void Traverse(Ray ray, ref float tmin, ref Trig triangle)
        {
            if (isLeaf)
            {
                FindIntersection(ray.Origin, ray.Dir, ref tmin, ref triangle);
                return;
            }

            if (!GetIntersection(ray.Origin, ray.Dir, box)) return;

            left?.Traverse(ray, ref tmin, ref triangle);
            middle?.Traverse(ray, ref tmin, ref triangle);
            right?.Traverse(ray, ref tmin, ref triangle);
        }

        private void FindIntersection(Vector3 origin, Vector3 dir, ref float tmin, ref Trig triangle)
        {
            foreach (var trig in trigs)
            {
                if (Geometry.ThereIsIntersection(origin, dir, trig, out var t))
                {
                    if (t > 0.01f && tmin > t)
                    {
                        tmin = t;
                        triangle = trig;
                    }
                }
            }
        }

        private bool GetIntersection(Vector3 origin, Vector3 dir, BoundingBox box)
        {
            var (tminX, tmaxX) = GetIntersection(origin, dir, box, 0);
            var (tminY, tmaxY) = GetIntersection(origin, dir, box, 1);
            var (tminZ, tmaxZ) = GetIntersection(origin, dir, box, 2);

            var tmin = Geometry.GetMax(tminX, tminY, tminZ);
            var tmax = Geometry.GetMin(tmaxX, tmaxY, tmaxZ);

            return tmin < tmax;
        }

        private (float tmin, float tmax) GetIntersection(Vector3 origin, Vector3 dir, BoundingBox box, int index)
        {
            var originDim = origin.GetByIndex(index);
            var dirDim = 1 / dir.GetByIndex(index);

            var tmin = (box.Mins.GetByIndex(index) - originDim) * dirDim;
            var tmax = (box.Maxs.GetByIndex(index) - originDim) * dirDim;

            return tmin < tmax ? (tmin, tmax) : (tmax, tmin);
        }

        public KdNode(List<Trig> trigs, int depth, int threshold)
        {
            this.trigs = trigs;
            UpdateBoundBox();
            Split(depth, threshold);
        }

        private void Split(int depth, int threshold)
        {
            depth--;
            var amount = trigs.Count;

            if (depth == 0 || amount < threshold)
            {
                isLeaf = true;
                return;
            }

            dimSplit = box.Size.GetIndexOfMaxValueOfThree();

            var (leftTrigs, middleTrigs, rightTrigs) = SplitTriangles();

            if (middleTrigs.Count != 0 && leftTrigs.Count == 0 && rightTrigs.Count == 0)
            {
                isLeaf = true;
                return;
            }

            trigs = null;

            if (middleTrigs.Count != 0) middle = new KdNode(middleTrigs, depth, threshold);
            if (leftTrigs?.Count != 0) left = new KdNode(leftTrigs, depth, threshold);
            if (rightTrigs?.Count != 0) right = new KdNode(rightTrigs, depth, threshold);
        }


        private (List<Trig> leftTrigs, List<Trig> middleTrigs, List<Trig> rightTrigs) SplitTriangles()
        {
            trigs.Sort((t1, t2) => Trig.CompareByPosition(t1, t2, dimSplit));

            var centerIndex = trigs.Count / 2 - 1;

            var centerTrig = trigs[centerIndex];
            var centerOfSplit = centerTrig.Center.GetByIndex(dimSplit);

            var startListSize = trigs.Count / 2;
            var leftTrigs = new List<Trig>(startListSize);
            var rightTrigs = new List<Trig>(startListSize);
            var middleTrigs = new List<Trig>(startListSize);

            foreach (var trig in trigs)
            {
                if (IsMiddle(trig, centerOfSplit))
                {
                    middleTrigs.Add(trig);
                }
                else if (trig.Center.GetByIndex(dimSplit) < centerOfSplit)
                {
                    leftTrigs.Add(trig);
                }
                else
                {
                    rightTrigs.Add(trig);
                }
            }

            return (leftTrigs, middleTrigs, rightTrigs);
        }

        private bool IsMiddle(Trig t, float center)
        {
            return (t.Mins.GetByIndex(dimSplit) <= center && t.Maxs.GetByIndex(dimSplit) >= center);
        }

        private void UpdateTriangleBoundBox(Trig t)
        {
            var newBox = new BoundingBox(t);
            MergeBoundBox(newBox);
        }

        private void MergeBoundBox(BoundingBox b)
        {
            box.Mins.X = Math.Min(box.Mins.X, b.Mins.X);
            box.Mins.Y = Math.Min(box.Mins.Y, b.Mins.Y);
            box.Mins.Z = Math.Min(box.Mins.Z, b.Mins.Z);

            box.Maxs.X = Math.Max(box.Maxs.X, b.Maxs.X);
            box.Maxs.Y = Math.Max(box.Maxs.Y, b.Maxs.Y);
            box.Maxs.Z = Math.Max(box.Maxs.Z, b.Maxs.Z);
            
            box.UpdateSize();
        }

        public void UpdateBoundBox()
        {
            var amount = trigs.Count;

            // to avoid 0 bounds when the BoundBox is at all zeros
            if (amount > 0) box.SetBounds(trigs[0]);

            for (var i = 1; i < amount; i++)
            {
                UpdateTriangleBoundBox(trigs[i]);
            }

            box.Mins -= Vector3.One * Epsilon;
            box.Maxs += Vector3.One * Epsilon;
        }
    }
}
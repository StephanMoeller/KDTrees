﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDTrees
{
    public interface IClosestPointFindStrategy
    {
        void Init(MapOfPoints mapOfPoints);
        List<Point> FindClosestPoints(Point point);
    }
}

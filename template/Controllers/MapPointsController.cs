using System;
using System.Collections.Generic;
using Connect.DNN.Modules.Map.Models.MapPoints;
using Connect.DNN.Modules.Map.Repositories;

namespace Connect.DNN.Modules.Map.Controllers
{

    public partial class MapPointsController
    {

        public static IEnumerable<MapPoint> GetMapPoints(int moduleId)
        {
            MapPointRepository repo = new MapPointRepository();
            return repo.Get(moduleId);
        }

        public static MapPoint GetMapPoint(int mapPointId, int moduleId)
        {
            MapPointRepository repo = new MapPointRepository();
            return repo.GetById(mapPointId, moduleId);
        }

        public static int AddMapPoint(ref MapPointBase mapPoint, int userId)
        {
            mapPoint.CreatedByUserID = userId;
            mapPoint.CreatedOnDate = DateTime.Now;
            mapPoint.LastModifiedByUserID = userId;
            mapPoint.LastModifiedOnDate = DateTime.Now;
            MapPointBaseRepository repo = new MapPointBaseRepository();
            repo.Insert(mapPoint);
            return mapPoint.MapPointId;
        }

        public static void UpdateMapPoint(MapPointBase mapPoint, int userId)
        {
            mapPoint.LastModifiedByUserID = userId;
            mapPoint.LastModifiedOnDate = DateTime.Now;
            MapPointBaseRepository repo = new MapPointBaseRepository();
            repo.Update(mapPoint);
        }

        public static void DeleteMapPoint(MapPointBase mapPoint)
        {
            MapPointBaseRepository repo = new MapPointBaseRepository();
            repo.Delete(mapPoint);
        }

    }
}

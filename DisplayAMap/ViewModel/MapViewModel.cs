using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Portal;

namespace DisplayAMap.ViewModel
{
    public class MapViewModel : INotifyPropertyChanged
    {
        public MapViewModel()
        {
            _ = SetupMap();
            CreateGraphics();

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) 
        { 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Map _map;

        public Map Map
        {
            get { return _map; }
            set { 
                    _map = value;
                    OnPropertyChanged();
                }
        }

        private GraphicsOverlayCollection _graphicsOverlays;
        public GraphicsOverlayCollection GraphicsOverlays
        {
            get { return _graphicsOverlays; }
            set
            {
                _graphicsOverlays = value;
                OnPropertyChanged();
            }
        }

        private async Task SetupMap()
        {

            // Create a new map with a 'topographic vector' basemap.
            Map = new Map(BasemapStyle.ArcGISTopographic);

            // Create an ArcGIS Portal object.
            ArcGISPortal portal = await ArcGISPortal.CreateAsync();

            // Create a portal item from the ArcGIS Portal object using a portal item string.
            PortalItem portalItem = await PortalItem.CreateAsync(portal, "2e4b3df6ba4b44969a3bc9827de746b3");

            // Create a feature layer from the portal item and specify a numerical layer id (i.e. 0).
            FeatureLayer layer = new FeatureLayer(portalItem, 0);

            // Add the layer to the operational layer of the map.
            Map.OperationalLayers.Add(layer);

            var parksUri = new Uri(
                "https://services3.arcgis.com/GVgbJbqm8hXASVYi/ArcGIS/rest/services/Parks_and_Open_Space/FeatureServer/0"
                );

            var trailsUri = new Uri(
                "https://services3.arcgis.com/GVgbJbqm8hXASVYi/ArcGIS/rest/services/Trails/FeatureServer/0"
                );

            var trailheadsUri = new Uri(
                "https://services3.arcgis.com/GVgbJbqm8hXASVYi/arcgis/rest/services/Trailheads/FeatureServer/0"
                );
            var parksLayer = new FeatureLayer(parksUri);
            var trailsLayer = new FeatureLayer(trailsUri);
            var trailheadsLayer = new FeatureLayer(trailheadsUri);

            Map.OperationalLayers.Add(parksLayer);
            Map.OperationalLayers.Add(trailsLayer);
            Map.OperationalLayers.Add(trailheadsLayer);


        }

        private void CreateGraphics()
        {
            // Create a new graphics overlay to contain a variety of graphics.
            var malibuGraphicsOverlay = new GraphicsOverlay();

            // Add the overlay to a graphics overlay collection.
            GraphicsOverlayCollection overlays = new GraphicsOverlayCollection
            {
                malibuGraphicsOverlay
            };

            // Set the view model's "GraphicsOverlays" property (will be consumed by the map view).
            this.GraphicsOverlays = overlays;

            // Create a point geometry.
            var dumeBeachPoint = new MapPoint(-118.8066, 34.0006, SpatialReferences.Wgs84);

            // Create a symbol to define how the point is displayed.
            var pointSymbol = new SimpleMarkerSymbol
            {
                Style = SimpleMarkerSymbolStyle.Circle,
                Color = System.Drawing.Color.Orange,
                Size = 10.0
            };

            // Add an outline to the symbol.
            pointSymbol.Outline = new SimpleLineSymbol
            {
                Style = SimpleLineSymbolStyle.Solid,
                Color = System.Drawing.Color.Blue,
                Width = 2.0
            };


            // Create a point graphic with the geometry and symbol.
            var pointGraphic = new Graphic(dumeBeachPoint, pointSymbol);

            // Add the point graphic to graphics overlay.
            malibuGraphicsOverlay.Graphics.Add(pointGraphic);

            // Create a list of points that define a polygon boundary.
            List<MapPoint> polygonPoints = new List<MapPoint>
            {
                new MapPoint(-118.8190, 34.0138, SpatialReferences.Wgs84),
                new MapPoint(-118.8068, 34.0216, SpatialReferences.Wgs84),
                new MapPoint(-118.7914, 34.0164, SpatialReferences.Wgs84),
                new MapPoint(-118.7960, 34.0086, SpatialReferences.Wgs84),
                new MapPoint(-118.8086, 34.0035, SpatialReferences.Wgs84)
            };

            // Create polygon geometry.
            var mahouRivieraPolygon = new Polygon(polygonPoints);

            // Create a fill symbol to display the polygon.
            var polygonSymbolOutline = new SimpleLineSymbol(SimpleLineSymbolStyle.Solid, System.Drawing.Color.Blue, 2.0);
            var polygonFillSymbol = new SimpleFillSymbol(SimpleFillSymbolStyle.Solid, System.Drawing.Color.Orange, polygonSymbolOutline);

            // Create a polygon graphic with the geometry and fill symbol.
            var polygonGraphic = new Graphic(mahouRivieraPolygon, polygonFillSymbol);

            // Add the polygon graphic to the graphics overlay.
            malibuGraphicsOverlay.Graphics.Add(polygonGraphic);

        }


    }
}

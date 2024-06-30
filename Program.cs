using Tools;
#region Hardcode Data
// 2.	Захардкодить эти данные в программу.
var a = new Vertex(3, 0);
var b = new Vertex(3, 1);
var c = new Vertex(3, 4);
var d = new Vertex(4, 5);
var e = new Vertex(4, 6);
var f = new Vertex(3, 6);
var g = new Vertex(2, 0);
var h = new Vertex(2, 2);
var i = new Vertex(2, 3);
var j = new Vertex(2, 4);
var k = new Vertex(2, 6);
var l = new Vertex(1, 5);

var green = new RailRoadPath("green");
var violet = new RailRoadPath("violet");
var white = new RailRoadPath("white");
var red = new RailRoadPath("red");

var greenEdges = new (Vertex, Vertex)[] { (a, b), (b, c), (c, f) };
var violetEdges = new (Vertex, Vertex)[] { (a, b), (b, h), (h, i), (i, j), (j, l) };
var whiteEdges = new (Vertex, Vertex)[] { (g, h), (h, i), (i, c), (c, d), (d, e) };
var redEdges = new (Vertex, Vertex)[] { (g, h), (h, i), (i, j), (j, k) };

foreach (var (from, to) in greenEdges) green.AddEdge(from, to);
foreach (var (from, to) in violetEdges) violet.AddEdge(from, to);
foreach (var (from, to) in whiteEdges) white.AddEdge(from, to);
foreach (var (from, to) in redEdges) red.AddEdge(from, to);

// 3. Реализовать алгоритм "заливки" парка.
var allInPark = new Park("allInPark");
var greenAndRedPark = new Park("greenAndRedPark");
var violetAndWhitePark = new Park("violetAndWhitePark");

var graphs = new[] { green, violet, white, red };
foreach (var graph in graphs) allInPark.AddGraph(graph);

greenAndRedPark.AddGraph(green);
greenAndRedPark.AddGraph(red);

violetAndWhitePark.AddGraph(violet);
violetAndWhitePark.AddGraph(white);

var parksList = new List<Park> { allInPark, greenAndRedPark, violetAndWhitePark };
#endregion

int parkId = Get.OneOfAvaliableParks(parksList);    // 4. Создать консольное приложение, которое выводит в виде списка доступные парки
var currentPark = parksList[parkId];
currentPark.PrintConvexHull();                      // и список вершин, описывающих парк.
currentPark.PrintAllRailRoadPaths();                // 3. Реализовать алгоритм "заливки" парка.
currentPark.PrintAllRailEdges();
Calculations.FindShortestPath(currentPark);         // 5. Поиск кратчайшего пути между участками
Console.ReadLine();

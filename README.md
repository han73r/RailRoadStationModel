Test Assignment
We are developing software for simulating the operation of a railway station.
Simplified, the station layout is a set of segments. Segments can be combined into tracks, and tracks, in turn, can belong to a certain yard.
Segments of tracks are line segments. Segments that share common points can be combined into polylines (tracks). A polyline can be associated with a certain entity (yard). What needs to be done.

1. Come up with and implement a data structure that will contain segments and points (station layout).
2. Hardcode this data into the program.
3. Implement the yard "filling" algorithm.
3.1. Each filling option includes a set of tracks (not segments!) belonging to a given yard and the yard's name. For example, "Yard 1": [Track A, Track B], "Yard 2": [Track C, Track D], "Yard 3": [Track E, Track F].
3.2. These options can also be hardcoded.
4. Create a console application that outputs a list of available yards and a list of vertices describing the yard.

Examples of filling (yard tracks highlighted in blue, filled yard in green):
5. Finding the shortest path between segments
5.1. Create a console application that outputs a list of all segments in the station layout: [Serial Number]. [Name].
5.2. Add the ability to select the starting and ending segment numbers.
5.3. Output a list of station segments included in the shortest path between the specified segments (or report that no path exists).
6. Write basic unit tests for the station layout, filling algorithm, and pathfinding.

Note
Tracks included in a yard may not have common points. There may be several tracks, or there may be only one.
The result of the filling algorithm is a shape representing the station yard.

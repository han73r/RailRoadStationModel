using System;
using System.Collections.Generic;
namespace Tools;
public static class Calculations
{
    public static void FindShortestPath(Park park) {
        List<Edge> allEdges = park.GetEdges();
        int startEdgeId = Get.ValidId(allEdges, edge => edge.Id, "Введите Id начального отрезка для расчёта кратчайшего расстояния между отрезками:");
        int endEdgeId = Get.ValidId(allEdges, edge => edge.Id, "Введите Id конечного отрезка:");
        Edge startEdge = allEdges.FirstOrDefault(e => e.Id == startEdgeId);
        Edge endEdge = allEdges.FirstOrDefault(e => e.Id == endEdgeId);
        // Извлекаем начальную и конечную вершины из выбранных рёбер
        (Vertex startVertex, Vertex endVertex) = park.ChooseVerticesForShortestPath(startEdge, endEdge);
        // Поиск всех рёбер между начальной и конечной вершинами
        (double shortestPathWeight, List<Edge> pathEdges) = park.FindEdgesBetweenVertices(startVertex, endVertex);                   // null!
        if (pathEdges.Count > 0) {
            // Вывод результатов
            Console.WriteLine($"Кратчайшее расстояние между отрезками {startEdge.Id} и {endEdge.Id}: {shortestPathWeight}");
            // Вывод всех рёбер между выбранными вершинами
            Console.WriteLine($"Отрезки пути входящие в кратчайшее расстояние между {startVertex.Id} и {endVertex.Id}:");
            foreach (var edge in pathEdges) {
                Console.WriteLine($"{edge.Id}: {edge.From.Id} -> {edge.To.Id}");

            }
        }
        else {
            Console.WriteLine("Между выбранными участками нет пути");
        }
    }
}
public static class Get
{
    public static int OneOfAvaliableParks(List<Park> parkList) {
        Console.WriteLine("Схема станции содержит следующие парки:");
        foreach (var park in parkList) {
            Console.WriteLine($"Id: {park.Id} \tname: {park.Name}");
        }
        int parkId = ValidId(parkList, park => park.Id, "Введите Id парка и нажмите Enter, чтобы продолжить");
        return parkId;
    }
    internal static int ValidId<T>(List<T> items, Func<T, int> idSelector, string prompt) {
        int id;
        bool isValid;
        do {
            Console.WriteLine(prompt);
            string input = Console.ReadLine();
            isValid = int.TryParse(input, out id) && items.Any(item => idSelector(item) == id);
            if (!isValid) {
                Console.WriteLine($"Неверный Id. Попробуйте снова.");
            }
        } while (!isValid);
        return id;
    }
}
public static class ConsoleExtensions
{
    public static void PrintCollection<T>(string header, IEnumerable<T> collection) {
        if (!string.IsNullOrEmpty(header)) {
            Console.WriteLine(header);
        }
        foreach (var item in collection) {
            Console.WriteLine(item);
        }
    }
}

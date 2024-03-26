// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;

class Chessboard
{
    /*VSTUP*/

    static string input; //cannot be int
    static List<string> inputArr = new List<string>(); //string[] temp; //fixed size length
    static string[] pair = new string[2];
    static int num_of_obstacles;
    static (int x, int y)[] obstacles;
    //static int[] start = new int[2];
    //static int[] end = new int[2];
    static (int x, int y) start = (0, 0);
    static (int x, int y) end = (0, 0);

    //Funkce, ktera prida do temp vice zadanych hodnot 
    static void AddMoreToInputArr()
    {
        int inputLength = 2 * num_of_obstacles + 5;

        while (inputArr.Count < inputLength)
        {
            input = Console.ReadLine();

            var tokens = input.Split();

            foreach (var token in tokens)
            {
                inputArr.Add(token);
            }
        }
    }
    static void AddObstacles()
    {
        for (int i = 0; i < num_of_obstacles; ++i)
        {
            pair = Console.ReadLine().Split();

            int xCord = int.Parse(pair[0]);
            int yCord = int.Parse(pair[1]);

            obstacles[i] = (xCord, yCord);
        }
    }
    static void initializeStart()
    {
        pair = Console.ReadLine().Split();

        start = (int.Parse(pair[0]), int.Parse(pair[1]));
    }
    static void initializeEnd()
    {
        pair = Console.ReadLine().Split();
        end = (int.Parse(pair[0]), int.Parse(pair[1]));
    }
    static void InitializeUsersInput()
    {
        //your input 

        //number of obstacles
        num_of_obstacles = int.Parse(Console.ReadLine());

        //int notConstNumObstacles = int.Parse(Console.ReadLine());

        //num_of_obstacles = notConstNumObstacles; 

        obstacles = new (int x, int y)[num_of_obstacles];

        //add pairs of obstacles to obstacles[] arr

        AddObstacles();

        //start and end coordinates

        initializeStart();
        initializeEnd();

    }

    /*NAJDI SOUSEDY PRO DANE POLICKO V SACHOVNICI*/

    //over, jestli dvojice (x,y) lezi na sachovnici 
    static bool IsValid(int x, int y) //chessboard 8x8 coordinates: from 0 to 7
    {
        return ((x > 0 & y > 0) & (x <= 8 & y <= 8));
    }

    //over, jestli obstacles (prekazky) obsahuje nejake policko
    static bool ObstaclesContains(int x, int y)
    {
        for (int i = 0; i < num_of_obstacles; i++)
        {
            if (obstacles[i] == (x, y))
            {
                return true;
            }
        }
        return false;
    }

    //najdi vsechny sousedy pro dane policko

    static List<(int x, int y)> FindNeighbours((int x, int y) vertex)
    {
        //vsechny mozne posuny z policka 
        (int x, int y)[] move_vectors = { (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1) };
        int x_f, y_f;
        int x0, y0;
        List<(int x, int y)> neighbours = new List<(int x, int y)>();


        for (int i = 0; i < move_vectors.GetLength(0); i++)
        {
            x0 = move_vectors[i].x;
            y0 = move_vectors[i].y;

            x_f = vertex.x + x0;
            y_f = vertex.y + y0;

            if (IsValid(x_f, y_f) & !ObstaclesContains(x_f, y_f))
            {
                (int x, int y) pair = (x_f, y_f);
                neighbours.Add(pair);
            }
        }

        return neighbours;
    }

    //BFS - najdi nejkratsi cestu 
    static string MakeIntArrayString((int x, int y) arr)
    {
        string key = $"{arr.x},{arr.y}";
        return key;
    }

    static void FindShortestPath()
    {

        //definuj promenne a datove struktury
        (int x, int y) cell;
        int depth;
        const int pathNotFound = -1;
        string endStr = $"{end.x},{end.y}";
        string key;

        Queue<((int x, int y), int)> queue = new Queue<((int x, int y), int)>();
        Dictionary<(int x, int y), int> visited = new Dictionary<(int x, int y), int>();
        Dictionary<(int x, int y), (int x, int y)> paths = new Dictionary<(int x, int y), (int x, int y)>();
        List<(int x, int y)> neighbours;
        List<(int x, int y)> track = new List<(int x, int y)>(); 
        




        queue.Enqueue((start, 0));

        //pokud fronta neni prazdna 
        while (queue.Count > 0)
        {
            (cell, depth) = queue.Dequeue();

            //add neighbours to queue
            neighbours = FindNeighbours(cell);

            foreach ((int x, int y) neighbour in neighbours)
            {
                //je to lepsi, kdyz klic neni pole, ale string, string jako klic se lepe hleda ve slovniku

                //pokud klic nebyl navstiven, pridame ho do fronty a do slovniku

                


                if (!visited.ContainsKey(cell))
                {
                    visited.Add(cell, depth);
                }


                if (!visited.ContainsKey(neighbour))
                {
                    visited.Add(neighbour, depth + 1);
                    queue.Enqueue((neighbour, depth + 1));

                    paths.Add(neighbour, cell); 
                   
                }
                //pokud klic se rovna cilovemu policku

                if (neighbour.Equals(end))
                {
                    //vratime hloubku policka, loop skonci 
                    //return visited[neighbour];
                    queue.Clear();
                    break; 
                }
            }
        }

        
        //vypis cestu 
        if (paths.ContainsKey(end))
        {
            (int x, int y) temp = end;
            track.Add(temp);
            while (temp != start)
            {
                temp = paths[temp];
                track.Add(temp);
            }

            for (int i = track.Count - 1; i > -1; i-- )
            {
                Console.WriteLine(track[i].x + " " + track[i].y); 
            }
        }
        else
        {
            Console.WriteLine(pathNotFound);
        }

        //return pathNotFound;
    }
    static public void Main(String[] args)
    {
        InitializeUsersInput();
        FindShortestPath(); 
    }
}
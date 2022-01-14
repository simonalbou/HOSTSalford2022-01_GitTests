using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSharpConcepts : MonoBehaviour
{
    // Collections allow handling multiple objects (or values) at once : 
    public GameObject[] squares; // An array of objects. Not resizable.
    public List<GameObject> listOfSquares; // A list of objects. Resizable.
    Queue<GameObject> queueOfSquares; // First-in-first-out (FIFO). Not serializable even if declared as public.
    Stack<GameObject> stackOfSquares; // Last-in-first-out (LIFO). Not serializable even if declared as public.

    void DoStuff() { } // dummy empty function we're going to call later

    void ReminderOnBooleanOperations()
    {
        bool cond1 = true;
        bool cond2 = false;

        if (cond1 && cond2) Debug.Log("1 and 2"); // this will not be executed unless BOTH are true.
        if (cond1 || cond2) Debug.Log("1 or 2"); // this works because at least one of them is true. (inclusive OR)
    }

    void Start()
    {
        Debug.Log(squares[0].name); // Use brackets to access an element
        Debug.Log(squares[999].name); // IndexOutOfRangeException
        Debug.Log(squares.Length); // Access the length of an array 

        // Two ways of initializing an array:
        string[] variousStrings = new string[42]; // ready to be populated
        string[] anotherPrivateArray = new string[]
        {
            "Monday",
            "Tuesday",
            "Wednesday"
        }; // don't forget the semicolon! It still counts as an instruction. Length is implicitly set to 3.

        anotherPrivateArray[1] = ""; // we can't delete a slot but we can empty it
        Transform[] someTransform = new Transform[3]; // for now, the three of them are equal to null
        someTransform[2] = transform; // we can set any value manually
        someTransform[2] = null; // we can set it back to null so it's an empty slot

        // Browsing a collection using a "for" loop:
        for (int i = 0; i < squares.Length; i++)
        {
            Transform currentItem = squares[i].transform;
            currentItem.Translate(Vector3.up * i);
        }

        // But wait, what's a loop?
        // "while" and "for" are the two possible keywords to set up a loop, which calls the same code several times
        int counter = 0;
        while (counter < 50)
        {
            DoStuff();
            counter++; // don't forget to make it so the "while" condition ends up being false at some point.

            // If we forget this, we get stuck in an endless loop.
            // An endless loop means Unity has to be restarted and we may lose our work.
        }

        // "for" loops save the trouble of setting up the counter, by embedding it into the "for" statement.
        for (int i = 0; i < 10; i++) // for (declaration; condition; incrementation)
        {
            // "continue" and "break" are precious keywords:
            if (i == 5) continue; // aborts the rest of this iteration and goes to the next one
            if (i == 7) break; // completely exits the current iteration AND the whole loop

            DoStuff(); // in the end, this function gets executed 6 times (0, 1, 2, 3, 4, 6)
        }

        // N-dimensional arrays also exist
        Transform[,] twoDimensionalGrid = new Transform[8,7];
        Debug.Log(twoDimensionalGrid.Length); // returns 56
        Transform[,,,,] truckloadOfDimensions; // yes, there's no limit.

        // Jagged arrays are often more convenient in game development.
        Transform[][] preferredTwoDimensionalGrid = new Transform[10][];
        // For instance this creates a 10*12 grid.
        // But we could choose to give each line a different length, which would have been impossible with a single 2D array. 
        for (int i = 0; i < preferredTwoDimensionalGrid.Length; i++)
        {
            preferredTwoDimensionalGrid[i] = new Transform[12];
        }

        // Browsing the N-dimensional structure is doable by nesting N loops (here with N=2)
        for (int i = 0; i < preferredTwoDimensionalGrid.Length; i++)
        {
            for (int j = 0; j < preferredTwoDimensionalGrid[i].Length; j++)
            {
                Debug.Log(preferredTwoDimensionalGrid[i][j].name);
            }
        }

        // Lists behave like arrays, but allow resizing, adding and removing
        List<string> listOfStrings = new List<string>();
        listOfStrings.Add("Monday");
        listOfStrings.Add("Tuesday");
        Debug.Log(listOfStrings.Count); // it's called Count and not Length. Will return 1 in this case.
        listOfStrings.Remove("Tuesday"); // finds a single element then removes it
        listOfStrings.RemoveAt(0); // removes the element located at this exact index
        listOfStrings.Clear(); // removes every element at once

        // Instead of Add/Remove, Queues have Enqueue/Dequeue to automatically handle element order. (FIFO)
        Queue<string> queueOfStrings = new Queue<string>();
        queueOfStrings.Enqueue("Monday");
        queueOfStrings.Enqueue("Tuesday");
        string firstElement = queueOfStrings.Dequeue(); // Dequeue returns the dequeued element so it can be used.
        string nextElement = queueOfStrings.Peek(); // returns the next element but without dequeuing it
        Debug.Log(firstElement); // prints Monday

        // Based on your game's needs, you can nest different types of collections together!
        Queue<CandyValue>[] matchThreeGrid;

        // Instead of Add/Remove, Stacks have Push/Pop to automatically handle element order. (LIFO)
        Stack<string> stackOfStrings = new Stack<string>();
        stackOfStrings.Push("Monday");
        stackOfStrings.Push("Tuesday");
        string lastElement = stackOfStrings.Pop(); // Pop returns the popped element so it can be used.
        string nextOne = stackOfStrings.Peek(); // returns the next element but without popping it
        Debug.Log(lastElement); // prints Tuesday
    }


    ///
    /// A more concrete example: object placement in a match-three game.
    /// 

    // Each tile should contain a CandyValue so we can know what's inside.
    enum CandyValue { Pink, Blue, Yellow, Hole }

    // represent the grid with nested arrays
    CandyValue[][] grid;

    // given the (x,y) tile, is it involved in a match?
    bool CheckForMatch(int x, int y)
    {
        // scanning for the surrounding tiles and checking whether they have the same value
        CandyValue center = grid[x][y];
        CandyValue twoLeft = grid[x-2][y];
        CandyValue oneLeft = grid[x-1][y];
        CandyValue oneRight = grid[x+1][y];
        CandyValue twoRight = grid[x+2][y];
        CandyValue twoDown = grid[x][y-2];
        CandyValue oneDown = grid[x][y-1];
        CandyValue oneUp = grid[x][y+1];
        CandyValue twoUp = grid[x][y+2];
        
        // Looking for matches
        if (center == oneLeft && center == twoLeft) return true;
        if (center == oneLeft && center == oneRight) return true;
        if (center == oneRight && center == twoRight) return true;
        if (center == oneDown && center == twoDown) return true;
        if (center == oneDown && center == oneUp) return true;
        if (center == oneUp && center == twoUp) return true;

        // no match has been found
        return false;
    }

    // Scanning for holes in one particular column (x) and making everything fall
    void ShiftEverythingDown(int x)
    {
        CandyValue[] column = grid[x];
        int holesEncountered = 0; // we'll keep track of how many holes we encountered

        // browse it beginning by the end so we go from bottom to top
        for (int i = column.Length-1; i <= 0; i--)
        {
            CandyValue currentCandy = column[i];

            // if there's no hole at all and no reason to fall, this iteration isn't interesting, we move on
            if (currentCandy != CandyValue.Hole && holesEncountered == 0) continue;

            // if we find a hole, there's nothing to move, but we remember it so subsequent candies will fall
            if (currentCandy == CandyValue.Hole)
            {
                holesEncountered++;
                continue;
            }

            // we found a candy and we saw a hole earlier: it should fall. We will copy its value N tiles below.
            column[i+holesEncountered] = currentCandy;

            // we just made the candy fall, so its slot should now be empty
            column[i] = CandyValue.Hole; // but it's not really necessary since the next iteration will make something fall here anyway
        }
    }
}

//Xixi Shi
namespace Kakurasu;

public class Program
{
    static void Main()
    {
        Console.CursorVisible = false;

        int[,] ex1 = new int[,] {
            { 0, 0, 0, 4}, // última col: suma por filas
            { 0, 0, 0, 5},
            { 0, 0, 0, 0},
            { 1, 2, 3, 0}  // ultima fil: suma por cols; el último 0 no cuenta
        };
        int[,] ex2 = new int[,] {
            { 0, 0, 0, 0, 7 },
            { 0, 0, 0, 0, 8 },
            { 0, 0, 0, 0, 5 },
            { 0, 0, 0, 0, 6 },
            { 3, 8, 5, 7, 0 }
        };

        // seleccion de ejemplo        
        int[,] mat = ex1;
        int N = mat.GetLength(0) - 1;

        char[,] tab = new char[N, N];  //matriz que codifica el estado de ñas casillas
        int[] obFil = new int[N];   //suma objetivo de fila
        int[] obCol = new int[N];   //suma objetivo de columna
        bool[] filIn = new bool[N];   //filas incorrectas
        bool[] colIn = new bool[N];   //columnas incorrectas

        //posición activa
        int fil;
        int col;   


        // inicialización y renderizado inicial
        Inizializa(mat, tab, obFil, obCol, out fil, out col);
        Render(tab, obFil, obCol, fil, col);

        bool esc = false;     //fin partida por escape
        bool comprobar = false;    //fin partida por completar
        // bucle principal
        while (!comprobar && !esc)
        {
            char tecla = LeeInput();
            if (tecla == 'q') //teclado escape
            {
                Console.SetCursorPosition(0, tab.GetLength(1) + 4);

                Console.WriteLine("¡Has salido del juego!");  //teclado q para salir
                esc = true;
            }
            else if (tecla == 'c') //teclado comprobar
            {
                var (filsInc, colsInc) = Incorrectas(tab, obFil, obCol);    //si solicita pista renderiza incorrectas
                RenderIncorrectas(tab, obFil, obCol, fil, col, filsInc, colsInc);
            }
            else
            {
                ProcesaInput(tecla, ref fil, ref col, tab);
                Render(tab, obFil, obCol, fil, col);    //renderiza tras cada movimiento

            }
            comprobar= Terminado(tab, obFil, obCol);   //comprobar 
        }
    }
    static void Inizializa(int[,] mat, char[,] tab, int[] obFil, int[] obCol, out int fil, out int col)
    {
        int N = tab.GetLength(0);
        for(int i=0; i<N; i++)  // mat tamaño (N +1)×(N +1)
        {
            obFil[i] = mat[i, N];
            obCol[i] = mat[N,i];
        }
        for (int i = 0; i < tab.GetLength(0); i++) {       //valor fila
            for (int j = 0; j < tab.GetLength(1); j++)     //valor columna
            {
                if (mat[i, j] == 0) tab[i, j] = ' ';        //matriz 0 = tab ' '
                else if (mat[i, j] == 1) tab[i, j] = 'X';   //matriz 1 = tab 'X'
                else tab[i, j] = '·';      //matriz 2 = tab '·'
            }
        }
        obFil = new int[] { 1, 2, 3 };
        obCol = new int[] { 4, 5, 0 };
        fil = col = 0;      // posición (0,0)
    }

    static void Render(char[,] tab, int[] obFil, int[] obCol, int fil, int col)
    {
        Console.Clear();
        //mostrar en pantalla descrita = for
        Console.Write("    ");   //espacio
        for (int i = 0; i < tab.GetLength(0); i++) Console.Write((i + 1)+" ");    //fila 1 2 3
        Console.WriteLine(" ");     //salto linea

        //línea delimitadora
        Console.Write("    ");   //espacio
        for (int i = 0; i < tab.GetLength(0); i++) Console.Write("__");        //fila linea delimitadora 
        Console.WriteLine(" ");     //salto linea

        for (int i = 0; i < tab.GetLength(0); i++)     //fila 1 2 3 + matriz + objetivo fila
        {
            Console.Write((i + 1) + " | ");
            for (int j = 0; j < tab.GetLength(1); j++)
            {
                Console.Write(tab[i, j]+ " ");
            }
            Console.WriteLine(" | " + obCol[i]);
        }

        //línea delimitadora
        Console.Write("    ");   //espacio
        for (int i = 0; i < tab.GetLength(0); i++) Console.Write("__");        //fila linea delimitadora 
        Console.WriteLine(" ");     //salto linea

        Console.Write("    ");   //espacio
        for (int i=0;i< obFil.Length; i++) Console.Write(obFil[i] + " ");        //objetivo columna

        Console.SetCursorPosition(col * 2 + 4 , fil + 2);    //posición casilla activa
        Console.ForegroundColor = ConsoleColor.White;
        Console.BackgroundColor = ConsoleColor.Green;
        Console.Write(tab[fil, col]);
        Console.ResetColor();
    }
    static char LeeInput()
    {
        char d = ' ';
        string tecla = Console.ReadKey(true).Key.ToString();
        switch (tecla)
        {
            case "LeftArrow": d = 'l'; break;
            case "UpArrow": d = 'u'; break;
            case "RightArrow": d = 'r'; break;
            case "DownArrow": d = 'd'; break;
            case "X": d = 'x'; break;  // marcar casilla
            case "V": d = 'v'; break;  // marcar casilla vacia
            case "C": d = 'c'; break;  // comprobar incorrectas
            case "Spacebar": d = 's'; break;  // limpiar casilla
            case "Escape": d = 'q'; break;  // terminar
            default: d = ' '; break;
        }
        return d;
    }
    static void ProcesaInput(char tecla, ref int fil, ref int col, char[,] tab)
    {
        int N = tab.GetLength(0);
        //posición casilla
        if (tecla == 'u') fil = (fil - 1 + N) % N; //mover arriba
        if (tecla == 'd') fil = (fil + 1) % N;      //mover abajo
        if (tecla == 'l') col = (col - 1 + N) % N;    //mover izquierda
        if (tecla == 'r') col = (col + 1) % N;      //mover derecha

        if (tecla == 'x') tab[fil, col] = 'X';      //marcar casilla negro
        if (tecla == 'v') tab[fil, col] = '·';      //marcar casilla vacia
        if (tecla == 's') tab[fil, col] = ' ';      //limpiar casilla
    }

    static int SumaFil(char[,] tab, int fil)        //calcula la suma de cada fila
    {
        int sum = 0;
        for (int i = 0; i < tab.GetLength(0); i++)
            if (tab[fil, i] == 'X') sum += (i+1);
        return sum;
    }

    static int SumaCol(char[,] tab, int col)        //calcula la suma de cada columna
    {
        int sum = 0;
        for (int j = 0; j < tab.GetLength(1); j++)
            if (tab[j,col ] == 'X') sum += (j+1);
        return sum;
    }

    static bool Terminado(char[,] tab, int[] objFil, int[] objCol)
    {
        int suma;
        int j = 0;
        bool correcta = true;
        while (correcta && j < tab.GetLength(1))  //comprobar resultado de suma coincida con el objetivo de las filas
        {
            suma = SumaFil(tab, j);
            if (suma != objCol[j]) correcta = false;
            j++;
        }
        if (!correcta) return false;
        else
        {
            int i = 0;
            while (correcta && i < tab.GetLength(0))  //comprobar resultado de suma coincida con el objetivo de las col
            {    
                suma = SumaCol(tab, i);
                if (suma != objFil[i]) correcta = false;
                j++;
            }
        }
        if (!correcta) return false;
        else  //si ambas son correctas termina el juego
        {
            Console.SetCursorPosition(0, tab.GetLength(1) + 4);
            Console.WriteLine("¡HAS GANADO!");
            return true;
        }
    }

    static (bool[], bool[]) Incorrectas(char[,] tab, int[] objFil, int[] objCol)
    {
        int N= tab.GetLength(0);
        //filas y columnas incorrectas
        bool[] filsInc = new bool[N];
        bool[] colsInc = new bool[N];

        for(int i = 0;i < N;i++) filsInc[i] = SumaFil(tab,i) != objFil[i];    //fInc= si la suma no es igual al objetivo fila
        for(int j = 0;j < N;j++) colsInc[j] = SumaCol(tab,j) != objCol[j];    //cInc= si la suma no es igual al objetivo col
        return (filsInc, colsInc);
    }

    static void RenderIncorrectas(char[,] tab, int[] objFil, int[] objCol,int fil, int col, bool[] filsInc, bool[] colsInc)
    {
        //renderizar el tablero
        Render(tab, objFil, objCol, fil, col);
        Console.SetCursorPosition(0, tab.GetLength(1) + 4);  //colocar el cursor debajo del renderizado para escribir pistas

        //indice de fila y columna incorrecta
        Console.Write("Filas incorrectas: ");
        for (int i = 0; i < filsInc.Length; i++)
            if (filsInc[i]==false) Console.Write("" + (i + 1));

        Console.WriteLine(" ");

        Console.Write("Columnas incorrectas: ");
        for (int j = 0; j < colsInc.Length; j++)
            if (colsInc[j]==false) Console.Write(""+  (j + 1));
        Console.WriteLine();
    }
}
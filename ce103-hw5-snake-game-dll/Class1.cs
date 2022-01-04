using System;
using System.IO;
using System.Threading;

namespace ce103_hw5_snake_dll
{
    public class snakedll
    {
        //we have defined some variables to use

        public const int SNAKE_ARRAY_SIZE = 310;
        public const ConsoleKey UP_ARROW = ConsoleKey.UpArrow;
        public const ConsoleKey LEFT_ARROW = ConsoleKey.LeftArrow;
        public const ConsoleKey RIGHT_ARROW = ConsoleKey.RightArrow;
        public const ConsoleKey DOWN_ARROW = ConsoleKey.DownArrow;
        public const ConsoleKey ENTER_KEY = ConsoleKey.Enter;
        public const ConsoleKey EXIT_BUTTON = ConsoleKey.Escape; // ESC
        public const ConsoleKey PAUSE_BUTTON = ConsoleKey.P; //p
        const char SNAKE_HEAD = (char)79;
        const char SNAKE_BODY = (char)58;
        const char WALL = (char)219;
        const char BLANK = ' ';

        public ConsoleKey waitForAnyKey()
        {
            ConsoleKey pressed; //we have defined pressed variable

            while (!Console.KeyAvailable) ;

            pressed = Console.ReadKey(false).Key;
            //pressed = tolower(pressed);
            return pressed;   //return pressed
        }

        public int getGameSpeed()
        {
            int speed = 0;  //we have defined speed variable
            Console.Clear();
            Console.SetCursorPosition(10, 5);      //we moved the cursor
            Console.Write("Select The game speed between 1 and 9 and press enter\n");
            Console.SetCursorPosition(10, 6);      //we moved the cursor
            int selection = Convert.ToInt32(Console.ReadLine());
            switch (selection)
            {
                case 1:
                    speed = 90;
                    break;
                case 2:
                    speed = 80;
                    break;
                case 3:
                    speed = 70;
                    break;
                case 4:
                    speed = 60;
                    break;
                case 5:
                    speed = 50;
                    break;
                case 6:
                    speed = 40;
                    break;
                case 7:
                    speed = 30;
                    break;
                case 8:
                    speed = 20;
                    break;
                case 9:
                    speed = 10;
                    break;

            }
            return speed; //return speed
        }

        public void pauseMenu()
        {

            Console.SetCursorPosition(28, 23);    //we moved the cursor
            Console.Write("**Paused**");

            waitForAnyKey();
            Console.SetCursorPosition(28, 23);    //we moved the cursor
            Console.Write("            ");
            return;
        }

        //This function checks if a key has pressed, then checks if its any of the arrow keys/ p/esc key. It changes direction acording to the key pressed.
        public ConsoleKey checkKeysPressed(ConsoleKey direction)
        {
            ConsoleKey pressed;  //we have defined pressed variable

            if (Console.KeyAvailable == true) //If a key has been pressed
            {
                pressed = Console.ReadKey(false).Key;
                if (direction != pressed)
                {
                    if (pressed == DOWN_ARROW && direction != UP_ARROW)
                    {
                        direction = pressed;
                    }
                    else if (pressed == UP_ARROW && direction != DOWN_ARROW)
                    {
                        direction = pressed;
                    }
                    else if (pressed == LEFT_ARROW && direction != RIGHT_ARROW)
                    {
                        direction = pressed;
                    }
                    else if (pressed == RIGHT_ARROW && direction != LEFT_ARROW)
                    {
                        direction = pressed;
                    }
                    else if (pressed == EXIT_BUTTON || pressed == PAUSE_BUTTON)
                    {
                        pauseMenu();
                    }
                }
            }
            return direction;   //return direction
        }
        //Cycles around checking if the x y coordinates ='s the snake coordinates as one of this parts
        //One thing to note, a snake of length 4 cannot collide with itself, therefore there is no need to call this function when the snakes length is <= 4
        public bool collisionSnake(int x, int y, int[,] snakeXY, int snakeLength, int detect)
        {
            int i;  //we have defined i variable
            for (i = detect; i < snakeLength; i++) //Checks if the snake collided with itself
            {
                if (x == snakeXY[0, i] && y == snakeXY[1, i])
                    return true;   //return true
            }
            return false;   //return false
        }
        //Generates food & Makes sure the food doesn't appear on top of the snake <- This sometimes causes a lag issue!!! Not too much of a problem tho
        public void generateFood(int[] foodXY, int width, int height, int[,] snakeXY, int snakeLength)
        {
            Random RandomNumbers = new Random();
            do
            {
                //RandomNumbers.Seed(time(null));
                foodXY[0] = RandomNumbers.Next() % (width - 2) + 2;
                //RandomNumbers.Seed(time(null));
                foodXY[1] = RandomNumbers.Next() % (height - 6) + 2;
            } while (collisionSnake(foodXY[0], foodXY[1], snakeXY, snakeLength, 0)); //This should prevent the "Food" from being created on top of the snake. - However the food has a chance to be created ontop of the snake, in which case the snake should eat it...

            Console.SetCursorPosition(foodXY[0], foodXY[1]);   //we moved the cursor
            Console.Write("@");
        }

        /*
        Moves the snake array forward, i.e. 
        This:
         x 1 2 3 4 5 6
         y 1 1 1 1 1 1
        Becomes This:
         x 1 1 2 3 4 5
         y 1 1 1 1 1 1

         Then depending on the direction (in this case west - left) it becomes:

         x 0 1 2 3 4 5
         y 1 1 1 1 1 1

         snakeXY[0][0]--; <- if direction left, take 1 away from the x coordinate
        */
        public void moveSnakeArray(int[,] snakeXY, int snakeLength, ConsoleKey direction)
        {
            int i;   //we have defined i variable
            for (i = snakeLength - 1; i >= 1; i--)
            {
                snakeXY[0, i] = snakeXY[0, i - 1];
                snakeXY[1, i] = snakeXY[1, i - 1];
            }

            /*
            because we dont actually know the new snakes head x y, 
            we have to check the direction and add or take from it depending on the direction.
            */
            switch (direction)
            {
                case DOWN_ARROW:
                    snakeXY[1, 0]++;
                    break;
                case RIGHT_ARROW:
                    snakeXY[0, 0]++;
                    break;
                case UP_ARROW:
                    snakeXY[1, 0]--;
                    break;
                case LEFT_ARROW:
                    snakeXY[0, 0]--;
                    break;
            }

            return;
        }

        /**
        *
        *	  @name   Move Snake Body (move)
        *
        *	  @brief Move snake body
        *
        *	  Moving snake body
        *
        *	  @param  [in] snakeXY [\b int[,]]  snake coordinates
        *	  
        *	  @param  [in] snakeLength [\b int]  index of fibonacci number in the serie
        *	  
        *	  @param  [in] direction [\b ConsoleKey]  index of fibonacci number in the serie
        **/
        public void move(int[,] snakeXY, int snakeLength, ConsoleKey direction)
        {
            int x;   //we have defined x variable
            int y;   //we have defined y variable

            //Remove the tail ( HAS TO BE DONE BEFORE THE ARRAY IS MOVED!!!!! )
            x = snakeXY[0, snakeLength - 1];
            y = snakeXY[1, snakeLength - 1];

            Console.SetCursorPosition(x, y);    //we moved the cursor
            Console.Write(BLANK);

            //Changes the head of the snake to a body part
            Console.SetCursorPosition(snakeXY[0, 0], snakeXY[1, 0]);   //we moved the cursor
            Console.Write(SNAKE_BODY);

            moveSnakeArray(snakeXY, snakeLength, direction);

            Console.SetCursorPosition(snakeXY[0, 0], snakeXY[1, 0]);    //we moved the cursor
            Console.Write(SNAKE_HEAD);

            Console.SetCursorPosition(1, 1); //Gets rid of the darn flashing underscore.

            return;
        }

        /**
        *
        *	  @name   eatfood (eat)
        *
        *	  @brief Snake eat food
        *
        *	  Eating @
        *
        *	  @param  [in] snakeXY [\b int[,]]  snake coordinates
        *	  
        *	  @param  [in] foodXY [\b int]  index of fibonacci number in the serie
        *	  
        *	  
        **/

        //This function checks if the snakes head his on top of the food, if it is then it'll generate some more food...
        public bool eatFood(int[,] snakeXY, int[] foodXY)
        {
            if (snakeXY[0, 0] == foodXY[0] && snakeXY[1, 0] == foodXY[1])
            {
                foodXY[0] = 0;
                foodXY[1] = 0; //This should prevent a nasty bug (loops) need to check if the bug still exists...

                return true;   //return true
            }

            return false;   //return false
        }

        /**
        *
        *	  @name   Collision Detection (console)
        *
        *	  @brief Detection of collision
        *
        *	  Collision Detection
        *
        *	  @param  [in] snakeXY [\b int[,]]  snake coordinates
        *	  
        *	  @param  [in] consoleWidth [\b int]  console witdh
        *	  
        *	  @param  [in] snakeLength [\b int]  snake length
        **/

        public bool collisionDetection(int[,] snakeXY, int consoleWidth, int consoleHeight, int snakeLength) //Need to Clean this up a bit
        {
            bool colision = false;   //we have defined colision variable
            if ((snakeXY[0, 0] == 1) || (snakeXY[1, 0] == 1) || (snakeXY[0, 0] == consoleWidth) || (snakeXY[1, 0] == consoleHeight - 4)) //Checks if the snake collided wit the wall or it's self
                colision = true;
            else
                if (collisionSnake(snakeXY[0, 0], snakeXY[1, 0], snakeXY, snakeLength, 1)) //If the snake collided with the wall, theres no point in checking if it collided with itself.
                colision = true;

            return colision;    //return colision
        }

        /**
        *
        *	  @name   Refresh Bar (refresh)
        *
        *	  @brief Refresh menu
        *
        *	  Refresh bar
        *
        *	  @param  [in] score [\b int]  snake coordinates
        *	  
        *	  @param  [in] speed [\b int]  index of fibonacci number in the serie
        *	  
        *	  
        **/
        public void refreshInfoBar(int score, int speed)
        {
            Console.SetCursorPosition(5, 23);    //we moved the cursor
            Console.Write("Score: " + score);

            Console.SetCursorPosition(5, 24);    //we moved the cursor
            switch (speed)
            {
                case 90:
                    Console.Write("Speed: 1");
                    break;
                case 80:
                    Console.Write("Speed: 2");
                    break;
                case 70:
                    Console.Write("Speed: 3");
                    break;
                case 60:
                    Console.Write("Speed: 4");
                    break;
                case 50:
                    Console.Write("Speed: 5");
                    break;
                case 40:
                    Console.Write("Speed: 6");
                    break;
                case 30:
                    Console.Write("Speed: 7");
                    break;
                case 20:
                    Console.Write("Speed: 8");
                    break;
                case 10:
                    Console.Write("Speed: 9");
                    break;
            }

            Console.SetCursorPosition(52, 23);    //we moved the cursor
            Console.Write("Coder: Ridvan KARASUBASI");

            Console.SetCursorPosition(52, 24);    //we moved the cursor
            Console.Write("Version: 0.5");

            return;
        }

        ////**************HIGHSCORE STUFF**************//

        ////-> The highscores system seriously needs to be clean. There are some bugs, entering a name etc

        public void createHighScores()
        {
            TextWriter file = new StreamWriter(@"..\\..\\..\\highscores.txt");
            int i;   //we have defined i variable
            if (file == null)
            {
                Console.Write("FAILED TO CREATE HIGHSCORES!!! EXITING!");
                Environment.Exit(0);
            }
            for (i = 0; i < 5; i++)
            {
                file.Write(i + 1);
                file.Write("\t0\t\t\tEMPTY\n");
            }
            file.Flush();
            file.Close();

            return;
        }

        //public int getLowestScore()
        //{
        //    FILE* fp;
        //    char[] str = new char[128];
        //    int lowestScore = 0;
        //    int i;
        //    int intLength;

        //    if ((fp = fopen("highscores.txt", "r")) == null)
        //    {
        //        //Create the file, then try open it again.. if it fails this time exit.
        //        createHighScores(); //This should create a highscores file (If there isn't one)
        //        if ((fp = fopen("highscores.txt", "r")) == null)
        //            Environment.Exit(1);
        //    }

        //    while (!feof(fp))
        //    {
        //        gets(str, 126, fp);
        //    }
        //    object p = close(fp);

        //    i = 0;

        //    //Gets the Int length
        //    while (str[2 + i] != '\t')
        //    {
        //        i++;
        //    }

        //    intLength = i;

        //    //Gets converts the string to int
        //    for (i = 0; i < intLength; i++)
        //    {
        //        lowestScore = lowestScore + ((int)str[2 + i] - 48) * pow(10, intLength - i - 1);
        //    }

        //    return (lowestScore);
        //}

        //public void inputScore(int score) //This seriously needs to be cleaned up
        //{
        //    FILE* fp;
        //    FILE* file;
        //    char[] str = new char[20];
        //    int fScore;
        //    int i, s, y;
        //    int intLength;
        //    int[] scores = new int[5];
        //    int x;
        //    char[] highScoreName = new char[20];
        //    char[,] highScoreNames = new char[5,20];

        //    char[] name = new char[20];

        //    int entered = 0;

        //    Console.Clear(); //clear the console

        //    if ((fp = fopen("highscores.txt", "r")) == null)
        //    {
        //        //Create the file, then try open it again.. if it fails this time exit.
        //        createHighScores(); //This should create a highscores file (If there isn't one)
        //        if ((fp = fopen("highscores.txt", "r")) == null)
        //            Environment.Exit(1);
        //    }
        //    Console.SetCursorPosition(10, 5);
        //    Console.Write("Your Score made it into the top 5!!!");
        //    Console.SetCursorPosition(10, 6);
        //    Console.Write("Please enter your name: ");
        //    gets(name);

        //    x = 0;
        //    while (!feof(fp))
        //    {
        //        fgets(str, 126, fp);  //Gets a line of text

        //        i = 0;

        //        //Gets the Int length
        //        while (str[2 + i] != '\t')
        //        {
        //            i++;
        //        }

        //        s = i;
        //        intLength = i;
        //        i = 0;
        //        while (str[5 + s] != '\n')
        //        {
        //            //Console.Write(str[5+s]);
        //            highScoreName[i] = str[5 + s];
        //            s++;
        //            i++;
        //        }
        //        //Console.Write("\n");

        //        fScore = 0;
        //        //Gets converts the string to int
        //        for (i = 0; i < intLength; i++)
        //        {
        //            //Console.Write(str[2+i]);
        //            fScore = fScore + ((int)str[2 + i] - 48) * pow(10, intLength - i - 1);
        //        }

        //        if (score >= fScore && entered != 1)
        //        {
        //            scores[x] = score;
        //            strcpy(highScoreNames[x], name);

        //            //Console.Write(x+1);
        //            //Console.Write(score, name);		
        //            x++;
        //            entered = 1;
        //        }

        //        //Console.Write("%d",x+1);
        //        //Console.Write("\t%d\t\t\t%s\n",fScore, highScoreName);
        //        //strcpy(text, text+"%d\t%d\t\t\t%s\n");
        //        strcpy(highScoreNames[x], highScoreName);
        //        scores[x] = fScore;

        //        //highScoreName = "";
        //        for (y = 0; y < 20; y++)
        //        {
        //            highScoreName[y] = 0x00; //NULL
        //        }

        //        x++;
        //        if (x >= 5)
        //            break;
        //    }

        //    fclose(fp);

        //    file = fopen("highscores.txt", "w+");

        //    for (i = 0; i < 5; i++)
        //    {
        //        //Console.Write(i+1, scores[i], highScoreNames[i]);
        //        Console.Write(file, i + 1, scores[i], highScoreNames[i]);
        //    }

        //    fclose(file);

        //    return;
        //}

        public void displayHighScores() //NEED TO CHECK THIS CODE!!!
        {
            string[] str = File.ReadAllLines(@"..\\..\\..\\highscores.txt");
            int y = 5;  //we have defined y variable
            Console.Clear();

            if (File.ReadAllLines(@"..\\..\\..\\highscores.txt") == null)
            {

                //Create the file, then try open it again.. if it fails this time exit.
                createHighScores(); //This should create a highscores file (If there isn't one)
                if (File.ReadAllLines(@"..\\..\\..\\highscores.txt") == null)
                    Environment.Exit(1);
            }

            Console.SetCursorPosition(10, y++);    //we moved the cursor
            Console.WriteLine("High Scores");
            Console.SetCursorPosition(10, y++);    //we moved the cursor
            Console.WriteLine("Rank\tScore\t\t\tName");
            for (int i = 0; i < str.Length; i++)
            {
                Console.SetCursorPosition(10, y++);    //we moved the cursor
                Console.Write(str[i]);
            }
            //Close the file
            Console.SetCursorPosition(10, y++);      //we moved the cursor
            Console.Write("Press any key to continue...");
            waitForAnyKey();
            return;
        }

        //**************END HIGHSCORE STUFF**************//

        public void youWinScreen()
        {
            Console.Clear();
            int x = 6, y = 7;        //we have defined x and y variables
            Console.SetCursorPosition(x, y++);    //we moved the cursor
            Console.Write("'##:::'##::'#######::'##::::'##::::'##:::::'##:'####:'##::: ##:'####:");
            Console.SetCursorPosition(x, y++);    //we moved the cursor
            Console.Write(". ##:'##::'##.... ##: ##:::: ##:::: ##:'##: ##:. ##:: ###:: ##: ####:");
            Console.SetCursorPosition(x, y++);    //we moved the cursor
            Console.Write(":. ####::: ##:::: ##: ##:::: ##:::: ##: ##: ##:: ##:: ####: ##: ####:");
            Console.SetCursorPosition(x, y++);    //we moved the cursor
            Console.Write("::. ##:::: ##:::: ##: ##:::: ##:::: ##: ##: ##:: ##:: ## ## ##:: ##::");
            Console.SetCursorPosition(x, y++);    //we moved the cursor
            Console.Write("::: ##:::: ##:::: ##: ##:::: ##:::: ##: ##: ##:: ##:: ##. ####::..:::");
            Console.SetCursorPosition(x, y++);    //we moved the cursor
            Console.Write("::: ##:::: ##:::: ##: ##:::: ##:::: ##: ##: ##:: ##:: ##:. ###:'####:");
            Console.SetCursorPosition(x, y++);    //we moved the cursor
            Console.Write("::: ##::::. #######::. #######:::::. ###. ###::'####: ##::. ##: ####:");
            Console.SetCursorPosition(x, y++);    //we moved the cursor
            Console.Write(":::..::::::.......::::.......:::::::...::...:::....::..::::..::....::");
            Console.SetCursorPosition(x, y++);    //we moved the cursor

            waitForAnyKey();
            Console.Clear(); //clear the console
            return;
        }

        /**
        *
        *	  @name   Game Over Screen 
        *
        *	  @brief Game Over Screen
        *
        *	  Game Over Screen
        *
        *	 
        **/
        public void gameOverScreen()
        {
            int x = 17, y = 3;  //we have defined x and y variables
            Console.Beep(2500, 275); //Beep
            Console.Clear();
            //http://www.network-science.de/ascii/ <- Ascii Art Gen

            Console.SetCursorPosition(x, y++);    //we moved the cursor
            Console.Write(":'######::::::'###::::'##::::'##:'########:\n");
            Console.SetCursorPosition(x, y++);    //we moved the cursor
            Console.Write("'##... ##::::'## ##::: ###::'###: ##.....::\n");
            Console.SetCursorPosition(x, y++);    //we moved the cursor
            Console.Write(" ##:::..::::'##:. ##:: ####'####: ##:::::::\n");
            Console.SetCursorPosition(x, y++);    //we moved the cursor
            Console.Write(" ##::'####:'##:::. ##: ## ### ##: ######:::\n");
            Console.SetCursorPosition(x, y++);    //we moved the cursor
            Console.Write(" ##::: ##:: #########: ##. #: ##: ##...::::\n");
            Console.SetCursorPosition(x, y++);    //we moved the cursor
            Console.Write(" ##::: ##:: ##.... ##: ##:.:: ##: ##:::::::\n");
            Console.SetCursorPosition(x, y++);    //we moved the cursor
            Console.Write(". ######::: ##:::: ##: ##:::: ##: ########:\n");
            Console.SetCursorPosition(x, y++);    //we moved the cursor
            Console.Write(":......::::..:::::..::..:::::..::........::\n");
            Console.SetCursorPosition(x, y++);    //we moved the cursor
            Console.Write(":'#######::'##::::'##:'########:'########::'####:\n");
            Console.SetCursorPosition(x, y++);    //we moved the cursor
            Console.Write("'##.... ##: ##:::: ##: ##.....:: ##.... ##: ####:\n");
            Console.SetCursorPosition(x, y++);    //we moved the cursor
            Console.Write(" ##:::: ##: ##:::: ##: ##::::::: ##:::: ##: ####:\n");
            Console.SetCursorPosition(x, y++);    //we moved the cursor
            Console.Write(" ##:::: ##: ##:::: ##: ######::: ########::: ##::\n");
            Console.SetCursorPosition(x, y++);    //we moved the cursor
            Console.Write(" ##:::: ##:. ##:: ##:: ##...:::: ##.. ##::::..:::\n");
            Console.SetCursorPosition(x, y++);    //we moved the cursor
            Console.Write(" ##:::: ##::. ## ##::: ##::::::: ##::. ##::'####:\n");
            Console.SetCursorPosition(x, y++);    //we moved the cursor
            Console.Write(". #######::::. ###:::: ########: ##:::. ##: ####:\n");
            Console.SetCursorPosition(x, y++);    //we moved the cursor
            Console.Write(":.......::::::...:::::........::..:::::..::....::\n");

            waitForAnyKey();
            Console.Clear(); //clear the console
            return;
        }

        /**
        *
        *	  @name   Start Game (start)
        *
        *	  @brief Start Game 
        *
        *	  Collision Detection
        *
        *	  @param  [in] snakeXY [\b int[,]]  snake coordinates
        *	  
        *	  @param  [in] consoleWidth [\b int]  console witdh
        *	  
        *	  @param  [in] consoleHeight [\b int]  console Height
        *	  
        *	   @param  [in] snakeLength [\b int]  snake length
        *	   
        *	   @param  [in] direction [\b Console Key]  direction
        *	   
        *
        *	   
        *	   @param  [in] score [\b int]  score
        *	   
        *	   @param  [in] speed [\b int]  speed
        **/

        //Messy, need to clean this function up
        public void startGame(int[,] snakeXY, int[] foodXY, int consoleWidth, int consoleHeight, int snakeLength, ConsoleKey direction, int score, int speed)
        {
            bool gameOver = false;   //we have defined gameOver variable
            ConsoleKey oldDirection = ConsoleKey.NoName;
            bool canChangeDirection = true;   //we have defined canChangeDirection variable
            int gameOver2 = 1;   //we have defined gameOver variable
            do
            {
                if (canChangeDirection)
                {
                    oldDirection = direction;
                    direction = checkKeysPressed(direction);
                }

                if (oldDirection != direction)//Temp fix to prevent the snake from colliding with itself
                    canChangeDirection = false;

                if (true) //haha, it moves according to how fast the computer running it is...
                {
                    //Console.SetCursorPosition(1,1);
                    //Console.Write(clock() , endWait);
                    move(snakeXY, snakeLength, direction);
                    canChangeDirection = true;


                    if (eatFood(snakeXY, foodXY))
                    {
                        generateFood(foodXY, consoleWidth, consoleHeight, snakeXY, snakeLength); //Generate More Food
                        snakeLength++;
                        switch (speed)
                        {
                            case 90:
                                score += 5;
                                break;
                            case 80:
                                score += 7;
                                break;
                            case 70:
                                score += 9;
                                break;
                            case 60:
                                score += 12;
                                break;
                            case 50:
                                score += 15;
                                break;
                            case 40:
                                score += 20;
                                break;
                            case 30:
                                score += 23;
                                break;
                            case 20:
                                score += 25;
                                break;
                            case 10:
                                score += 30;
                                break;
                        }

                        refreshInfoBar(score, speed);
                    }
                    
                    Thread.Sleep(speed);
                }

                gameOver = collisionDetection(snakeXY, consoleWidth, consoleHeight, snakeLength);

                if (snakeLength >= SNAKE_ARRAY_SIZE - 5) //Just to make sure it doesn't get longer then the array size & crash
                {
                    gameOver2 = 2;//You Win! <- doesn't seem to work - NEED TO FIX/TEST THIS
                    score += 1500; //When you win you get an extra 1500 points!!!
                }

            } while (!gameOver);

            switch (gameOver2)
            {
                case 1:
                    gameOverScreen();

                    break;
                case 2:
                    youWinScreen();
                    break;
            }

            //if (score >= getLowestScore() && score != 0)
            //{
            //    inputScore(score);
            //    displayHighScores();
            //}

            return;
        }

        /**
        *
        *	  @name   Load Environment (environment)
        *
        *	  @brief Load environment
        *
        *	  Load Environment
        *
        *	  @param  [in] consoleWitdh [\b int]  consoleWitdh
        *	  
        *	  @param  [in] consoleHeight [\b int]  consoleHeight
        *	  
        *	  
        **/

        public void loadEnviroment(int consoleWidth, int consoleHeight)//This can be done in a better way... FIX ME!!!! Also i think it doesn't work properly in ubuntu <- Fixed
        {

            int x = 1, y = 1;   //we have defined x and y variables
            int rectangleHeight = consoleHeight - 4;
            Console.Clear(); //clear the console

            Console.SetCursorPosition(x, y); //Top left corner

            for (; y < rectangleHeight; y++)
            {
                Console.SetCursorPosition(x, y); //Left Wall 
                Console.Write("|", WALL);   //we created game walls

                Console.SetCursorPosition(consoleWidth, y); //Right Wall
                Console.Write("|",WALL);    //we created game walls
            }

            y = 1;
            for (; x < consoleWidth + 1; x++)
            {
                Console.SetCursorPosition(x, y); //Left Wall 
                Console.Write("-", WALL);    //we created game walls

                Console.SetCursorPosition(x, rectangleHeight); //Right Wall
                Console.Write("-", WALL);    //we created game walls
            }

            /*
                for (i = 0; i < 80; i++)
                {
                    Console.Write(WALL);
                }

                for (i = 0; i < 17; i++)
                {
                    Console.Write(WALL);
                }

                for (i = 0; i < 21; i++)
                {
                    Console.Write(WALL);
                    Console.SetCursorPosition(80,i);
                }

                for (i = 0; i < 81; i++)
                {
                    Console.Write(WALL);
                }	
            */
            return;
        }

        /**
        *
        *	  @name   Load Snake (Snake)
        *
        *	  @brief Load Snake
        *
        *	  Load Environment
        *
        *	  @param  [in] snakeXY [\b int]  snakeXY
        *	  
        *	  @param  [in] snakeLength [\b int]  snakeLength
        *	  
        *	  
        **/

        public void loadSnake(int[,] snakeXY, int snakeLength)
        {
            int i;   //we have defined i variable
            /*
            First off, The snake doesn't actually have enough XY coordinates (only 1 - the starting location), thus we use
            these XY coordinates to "create" the other coordinates. For this we can actually use the function used to move the snake.
            This helps create a "whole" snake instead of one "dot", when someone starts a game.
            */
            //moveSnakeArray(snakeXY, snakeLength); //One thing to note ATM, the snake starts of one coordinate to whatever direction it's pointing...

            //This should print out a snake :P
            for (i = 0; i < snakeLength; i++)
            {
                Console.SetCursorPosition(snakeXY[0, i], snakeXY[1, i]);
                Console.Write(SNAKE_BODY); //Meh, at some point I should make it so the snake starts off with a head...
            }

            return;
        }

        /* NOTE, This function will only work if the snakes starting direction is left!!!! 
        Well it will work, but the results wont be the ones expected.. I need to fix this at some point.. */

        /**
        *
        *	  @name   prepairSnakeArray (prepair snake)
        *
        *	  @brief Prepair Snake Array
        *
        *	  Prepair Snake Array
        *
        *	  @param  [in] snakeXY [\b int]  snakeXY
        *	  
        *	  @param  [in] snakeLength [\b int]  snakeLength
        *	  
        *	  
        **/

        public void prepairSnakeArray(int[,] snakeXY, int snakeLength)
        {
            int i;   //we have defined i variable
            int snakeX = snakeXY[0, 0];    //We defined array value
            int snakeY = snakeXY[1, 0];    //We defined array value

            // this is used in the function move.. should maybe create a function for it...
            /*switch(direction)
            {
                case DOWN_ARROW:
                    snakeXY[1][0]++;
                    break;
                case RIGHT_ARROW:
                    snakeXY[0][0]++;
                    break;
                case UP_ARROW:
                    snakeXY[1][0]--;
                    break;
                case LEFT_ARROW:
                    snakeXY[0][0]--;
                    break;			
            }
            */


            for (i = 1; i <= snakeLength; i++)
            {
                snakeXY[0, i] = snakeX + i;    //We defined array value
                snakeXY[1, i] = snakeY;      //We defined array value
            }

            return;
        }

        //This function loads the enviroment, snake, etc

        /**
        *
        *	  @name   Load Game (Load Game)
        *
        *	  @brief Load Game
        *
        *	  Load Game
        *
        *	  
        **/

        public void loadGame()
        {
            int[,] snakeXY = new int[2, SNAKE_ARRAY_SIZE]; //Two Dimentional Array, the first array is for the X coordinates and the second array for the Y coordinates

            int snakeLength = 4; //Starting Length

            ConsoleKey direction = ConsoleKey.LeftArrow; //DO NOT CHANGE THIS TO RIGHT ARROW, THE GAME WILL INSTANTLY BE OVER IF YOU DO!!! <- Unless the prepairSnakeArray function is changed to take into account the direction....

            int[] foodXY = { 5, 5 };// Stores the location of the food

            int score = 0;  //we defined score variable
            //int level = 1;

            //Window Width * Height - at some point find a way to get the actual dimensions of the console... <- Also somethings that display dont take this dimentions into account.. need to fix this...
            int consoleWidth = 80;   //We defined consoleWidth variable
            int consoleHeight = 25;   //we defined consoleHeight

            int speed = getGameSpeed();   //we defined speed

            //The starting location of the snake
            snakeXY[0, 0] = 40;   //We defined array value
            snakeXY[1, 0] = 10;   //We defined array value

            loadEnviroment(consoleWidth, consoleHeight); //borders
            prepairSnakeArray(snakeXY, snakeLength);
            loadSnake(snakeXY, snakeLength);
            generateFood(foodXY, consoleWidth, consoleHeight, snakeXY, snakeLength);
            refreshInfoBar(score, speed); //Bottom info bar. Score, Level etc
            startGame(snakeXY, foodXY, consoleWidth, consoleHeight, snakeLength, direction, score, speed);

            return;
        }

        //**************MENU STUFF**************//

        /**
        *
        *	  @name   menuSelector (menuSelector)
        *
        *	  @brief menu Selector
        *
        *	  Menu Selector
        *
        *	  @param  [in] x [\b int]  x
        *	  
        *	  @param  [in] y [\b int]  y
        *	  
        *	  @param  [in] yStart [\b int]  yStart
        *	  
        *	  
        **/

        public int menuSelector(int x, int y, int yStart)
        {
            char key;    //we have defined key variable
            int i = 0;   //we have defined i variable
            x = x - 2;   //we defined x variable
            Console.SetCursorPosition(x, yStart);

            Console.Write(">");  

            Console.SetCursorPosition(1, 1);


            do
            {
                key = (char)waitForAnyKey();
                //Console.Write(key, (int)key);
                if (key == (char)UP_ARROW)
                {
                    Console.SetCursorPosition(x, yStart + i);
                    Console.Write(" ");    //we wrote the blank

                    if (yStart >= yStart + i)
                        i = y - yStart - 2;
                    else
                        i--;
                    Console.SetCursorPosition(x, yStart + i);
                    Console.Write(">");   //we wrote the ">"
                }
                else
                    if (key == (char)DOWN_ARROW)
                {
                    Console.SetCursorPosition(x, yStart + i);
                    Console.Write(" ");   //we wrote the blank

                    if (i + 2 >= y - yStart)
                        i = 0;
                    else
                        i++;
                    Console.SetCursorPosition(x, yStart + i);
                    Console.Write(">");
                }
                //Console.SetCursorPosition(1,1);
                //Console.Write("%d", key);
            } while (key != (char)ENTER_KEY); //While doesn't equal enter... (13 ASCII code for enter) - note ubuntu is 10
            return i;    //return i
        }

        /**
        *
        *	  @name   Welcome Art (Welcome Art)
        *
        *	  @brief Welcome Art
        *
        *	  Welcome Art
        *
        *	  
        **/

        public void welcomeArt()
        {
            Console.Clear(); //clear the console
                             //Ascii art reference: http://www.chris.com/ascii/index.php?art=animals/reptiles/snakes
            Console.Write("\n");
            Console.Write("\t\t    _________         _________ 			\n");
            Console.Write("\t\t   /         \\       /         \\ 			\n");
            Console.Write("\t\t  /  /~~~~~\\  \\     /  /~~~~~\\  \\ 			\n");
            Console.Write("\t\t  |  |     |  |     |  |     |  | 			\n");
            Console.Write("\t\t  |  |     |  |     |  |     |  | 			\n");
            Console.Write("\t\t  |  |     |  |     |  |     |  |         /	\n");
            Console.Write("\t\t  |  |     |  |     |  |     |  |       //	\n");
            Console.Write("\t\t (o  o)    \\  \\_____/  /     \\  \\_____/ / 	\n");
            Console.Write("\t\t  \\__/      \\         /       \\        / 	\n");
            Console.Write("\t\t    |        ~~~~~~~~~         ~~~~~~~~ 		\n");
            Console.Write("\t\t    ^											\n");
            Console.Write("\t		Welcome To The Snake Game!			\n");
            Console.Write("\t			    Press Any Key To Continue...	\n");
            Console.Write("\n");

            waitForAnyKey();
            return;
        }

        /**
        *
        *	  @name   Controls (Controls)
        *
        *	  @brief Controls of game
        *
        *	  Game Control
        *
        *	  
        **/

        public void controls()
        {
            int x = 10, y = 5;    //we have defined x and y variables
            Console.Clear(); //clear the console
            Console.SetCursorPosition(x, y++);
            Console.Write("\t\t-- Controls --");
            Console.SetCursorPosition(x++, y++);
            Console.Write(" Use the following arrow keys to direct the snake to the food: ");
            Console.SetCursorPosition(x, y++);
            Console.Write("- Right Arrow");
            Console.SetCursorPosition(x, y++);
            Console.Write("- Left Arrow");
            Console.SetCursorPosition(x, y++);
            Console.Write("- Top Arrow");
            Console.SetCursorPosition(x, y++);
            Console.Write("- Bottom Arrow");
            Console.SetCursorPosition(x, y++);
            Console.SetCursorPosition(x, y++);
            Console.Write("- P & Esc pauses the game.");
            Console.SetCursorPosition(x, y++);
            Console.SetCursorPosition(x, y++);
            Console.Write("Press any key to continue...");
            Console.ReadKey();
            return;
        }

        /**
        *
        *	  @name   exitXY (exit)
        *
        *	  @brief Exit from game
        *
        *	  Exit Game
        *
        *	  
        **/

        public void exitYN()
        {
            char pressed;    //we have defined pressed variable
            Console.SetCursorPosition(9, 8);
            Console.Write("Are you sure you want to exit(Y/N)\n");

            do
            {
                pressed = (char)waitForAnyKey();
                pressed = char.ToLower(pressed);
            } while (!(pressed == 'y' || pressed == 'n'));

            if (pressed == 'y')
            {
                Console.Clear(); //clear the console
                Environment.Exit(0);
            }
            return;
        }

        /**
        *
        *	  @name  Main Menu (Main Menu)
        *
        *	  @brief Main Menu
        *
        *	  Main Menu
        *
        *	  
        **/

        public int mainMenu()
        {
            int x = 10, y = 5;   //we have defined x and y variables
            int yStart = y;     //we have defined yStart variable

            int selected;    //we have defined selected variable

            Console.Clear(); //clear the console
                             //Might be better with arrays of strings???
            Console.SetCursorPosition(x, y++);
            Console.Write("New Game\n");
            Console.SetCursorPosition(x, y++);
            Console.Write("High Scores\n");
            Console.SetCursorPosition(x, y++);
            Console.Write("Controls\n");
            Console.SetCursorPosition(x, y++);
            Console.Write("Exit\n");
            Console.SetCursorPosition(x, y++);

            selected = menuSelector(x, y, yStart);

            return selected;    //return selected
        }

        //**************END MENU STUFF**************//
        public int main() //Need to fix this up
        {

            welcomeArt();
            Console.CursorVisible = false;
            do
            {
                switch (mainMenu())
                {
                    case 0:
                        loadGame();
                        break;
                    case 1:
                        displayHighScores();
                        break;
                    case 2:
                        controls();
                        break;
                    case 3:
                        exitYN();
                        break;
                }
            } while (true);    
        }
    }
}
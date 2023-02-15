using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using SkiaSharp.Views;
using LiveChartsCore.Defaults;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using SkiaSharp;
using System.Diagnostics;

namespace NEA4
{
    public struct Token
    {
        public string type;
        public string contents;
        public TreeNode[] tree;
        public int bracketDepth;

    }
    public class Parsing
    {
        private string pInput;
        private int greatestDepth;
        private TreeNode[] Tree;

        private char[] charOperationArray = { '^', '*', '/', '-', '+' };
        public Parsing(string userInput)
        {
            pInput = userInput;
            Token[] tokenArray = RemoveBrackets(BracketDepth(ImplicitNegative(ImplicitMultiplication(Lexer(pInput)))));
            greatestDepth = GreatestDepth(tokenArray);
            Tree = Parser(tokenArray);
            Debug.WriteLine("Breakpoint");
        }

        public TreeNode GetTree()
        {
            return Tree[0];
        }

        private Token[] Lexer(string input) //Each token is added to the token list when the next character is about to be lexed. No use for characterLexed currently, but useful if multiple passes are needed for more complex handling
        {
            List<Token> tokens = new List<Token>();
            List<string> tokennames = new List<string>();
            List<string> stringtokens = new List<string>();

            bool[] characterLexed = new bool[input.Length];

            while (Char.IsWhiteSpace(input[0]))
            {
                input = input.Substring(1);
            }
            string stringtoken = string.Empty;
            string lasttokenname = string.Empty;

            for (int i = 0; i < input.Length; i++)
            {
                if (Char.IsDigit(input[i])) // IF DIGIT
                {
                    if (lasttokenname != "number")
                    {
                        if (stringtoken != null)
                        {
                            stringtokens.Add(stringtoken);
                            tokennames.Add(lasttokenname);

                        }
                        lasttokenname = "number";

                        stringtoken = string.Empty + input[i];

                    }
                    else
                    {
                        stringtoken = stringtoken + input[i];

                    }
                    characterLexed[i] = true;
                }
                else if (input[i] == '(' || input[i] == ')')
                {
                    if (stringtoken != null)
                    {
                        stringtokens.Add(stringtoken);
                        tokennames.Add(lasttokenname);
                    }
                    stringtoken = string.Empty + input[i];
                    
                    lasttokenname = "bracket";
                }

                else if (Char.IsLetter(input[i]))
                {

                    if (Char.IsUpper(input[i]) || input[i] == 'x') //treat as variable
                    {
                        if (stringtoken != null)
                        {
                            stringtokens.Add(stringtoken);
                            tokennames.Add(lasttokenname);

                        }
                        lasttokenname = "variable";
                        stringtoken = string.Empty + input[i];
                        characterLexed[i] = true;

                    }
                    else //treat as function - cos, sin
                    {
                        if (lasttokenname != "function")
                        {
                            if (stringtoken != null)
                            {
                                stringtokens.Add(stringtoken);
                                tokennames.Add(lasttokenname);
                                
                            }
                            lasttokenname = "function";
                            stringtoken = string.Empty + input[i];

                        }
                        else
                        {
                            stringtoken = stringtoken + input[i];

                        }
                        characterLexed[i] = true;
                    }

                }
                else //treat as operation
                {
                    if (stringtoken != null)
                    {
                        stringtokens.Add(stringtoken);
                        tokennames.Add(lasttokenname);

                    }
                    lasttokenname = "operation";

                    stringtoken = string.Empty + input[i];

                    characterLexed[i] = true;
                }
            }

            if (stringtoken == null)
            {
                throw new Exception("Last token was null");
            }
            else
            {
                stringtokens.Add(stringtoken);
                tokennames.Add(lasttokenname);
            }

            for (int i = stringtokens.Count-1; i > -1; i--) //removing whitespace tokens
            {
                if(string.IsNullOrWhiteSpace(stringtokens[i]))
                {
                    stringtokens.RemoveAt(i);
                    tokennames.RemoveAt(i);

                }
            }



            if(stringtokens.Count == tokennames.Count)
            {
                for (int i = 0; i < stringtokens.Count; i++) //CONVERT INTO SEMANTIC
                {
                    Token temp = new Token();
                    temp.contents = stringtokens[i];
                    temp.type = tokennames[i];
                    tokens.Add(temp);
                }
            }
            else
            {
                throw new Exception("number of tokens does not match number of token types");
            }



            Token[] tokenArray = ListToArray(tokens);
            return tokenArray;

        }

        private int GreatestDepth(Token[] input)
        {
            int localGreatestDepth = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i].bracketDepth > localGreatestDepth)
                {
                    localGreatestDepth = input[i].bracketDepth;
                }
            }
            return localGreatestDepth;
        }

        private Token[] ImplicitMultiplication(Token[] input)
        {
            List<Token> temp = ArrayToList(input);

            Token mult = new Token();
            mult.contents = "*";
            mult.type = "operation";

            for (int i = 0; i < temp.Count-1; i++)
            {
                if(temp[i].type != "operation" && temp[i+1].type != "operation")
                {
                    if(temp[i].type == "function" || temp[i].contents == "(" || temp[i+1].contents == ")")
                    {

                    }
                    else
                    {
                        temp.Insert(i + 1, mult);
                        
                    }
                }
            }
            Token[] output = new Token[temp.Count];
            for (int i = 0; i < output.Length; i++)
            {
                output[i] = temp[i];
            }
            return output;





        }
        
        private Token[] BracketDepth(Token[] input)
        {
            bool[] processedBracket = new bool[input.Length];
            int[] bracketDepthatIndex = new int[input.Length]; //0 means no brackets
            for (int i = 0; i < input.Length; i++)
            {
                bracketDepthatIndex[i] = 0;
            }


            int bracketsDepth = 0;

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i].contents == "(")
                {
                    bracketsDepth++;
                    processedBracket[i] = true;
                    bracketDepthatIndex[i] = bracketsDepth;

                }

                else if (input[i].contents == ")")
                {

                    processedBracket[i] = true;
                    bracketDepthatIndex[i] = bracketsDepth;
                    bracketsDepth--;

                }
                else
                {
                    bracketDepthatIndex[i] = bracketsDepth;
                }


            }

            for (int i = 0; i < input.Length; i++)
            {
                input[i].bracketDepth = bracketDepthatIndex[i];
            }
            return input;
        }

        private Token[] RemoveBrackets(Token[] input)
        {
            List<Token> tempList = new List<Token>(); //removing brackets
            for (int i = 0; i < input.Length; i++)
            {
                if ((string)input[i].contents != "(" && (string)input[i].contents != ")")
                {
                    tempList.Add(input[i]);

                }
            }

            return ListToArray(tempList);
        }
        
        private TreeNode[] Parser(Token[] input) //BODMAS - BRACKETS, INDICES, DIVISION, MULTIPLICATION, ADDITION, SUBTRACTION  // should be recursive
        {
            int localGreatestDepth = GreatestDepth(input);

            List<Token> inputList = ArrayToList(input);   
            
            TreeNode[] treeNodeArray = new TreeNode[inputList.Count];
            bool noBrackets = true;
            for (int i = 0; i < inputList.Count; i++)
            {
                if(inputList[i].bracketDepth != localGreatestDepth)
                {
                    noBrackets = false;
                }
            }
            if(!noBrackets)
            {
                for (int i = 0; i < inputList.Count; i++)
                {
                    if (inputList[i].bracketDepth == localGreatestDepth) //MAKE ALL TOKENS OF SAME DEPTH INTO ONE TOKEN BEFORE THE OUTER FOR LOOP REPEATS
                    {
                        
                        List<Token> temp = new List<Token>();
                        int storedBracketDepth = localGreatestDepth;
                        int z = i;
                        while (z < inputList.Count && inputList[z].bracketDepth == localGreatestDepth)
                        {
                            temp.Add(inputList[z]);
                            z++;
                        }
                        for (int l = 0; l < (z-i); l++)
                        {
                            inputList.RemoveAt(i);
                        }
                        Token[] recursiveInput = ListToArray(temp);
                        Token treeNodeInsert = new Token();
                        treeNodeInsert.tree = Parser(recursiveInput);
                        treeNodeInsert.type = "tree";
                        treeNodeInsert.bracketDepth = storedBracketDepth + 1;
                        inputList.Insert(i, treeNodeInsert);
                        
                        localGreatestDepth = GreatestDepth(ListToArray(inputList));
                        
                    }
                }
            }
            for (int i = inputList.Count-1; i >-1; i--)
            {
                if(inputList[i].type == "function")
                {
                    TreeNode functionAsParent = new TreeNode(inputList[i], null, null);

                    int index = FindRoot(inputList[i + 1].tree);

                    inputList[i + 1].tree[index].SetParent(inputList[i + 1].tree[index], functionAsParent);
                    //treeNode[] concatenatedArray = new treeNode[inputList[i + 1].tree.Length + 1];
                    //for (int q = 0; q < inputList[i + 1].tree.Length; q++)
                    //{
                    //    concatenatedArray[i] = inputList[i + 1].tree[q];
                    //}
                    //concatenatedArray[concatenatedArray.Length - 1] = functionAsParent;
                    //treeNodeInsert.tree = concatenatedArray;
                    TreeNode[] root = new TreeNode[1];
                    root[0] =  functionAsParent;

                    Token treeNodeInsert = new Token();
                    treeNodeInsert.type = "tree";
                    treeNodeInsert.tree = root;
                    inputList[i] = treeNodeInsert;
                    inputList.RemoveAt(i + 1);

                }
            }
            bool noOperation = true;
            foreach (char operationChar in charOperationArray)
            {
                string operation = string.Empty + operationChar;
                for (int i = 0; i < inputList.Count; i++)
                {
                    if(inputList[i].contents == operation)
                    {
                        noOperation = false;
                        List<TreeNode> leftList = new List<TreeNode>();
                        List<TreeNode> rightList = new List<TreeNode>();
                        TreeNode parentNode = new TreeNode(inputList[i], null, null);
                        if(inputList[i-1].type == "tree")
                        {
                            int index = FindRoot(inputList[i - 1].tree);
                            inputList[i - 1].tree[index].side = "left";
                            inputList[i - 1].tree[index].SetParent(inputList[i - 1].tree[index], parentNode);
                            for (int r = 0; r < inputList[i-1].tree.Length; r++)
                            {
                                leftList.Add(inputList[i - 1].tree[r]);
                            }
                            
                        }
                        else
                        {
                            TreeNode leftChild = new TreeNode(inputList[i - 1], parentNode, "left");
                            leftList.Add(leftChild);
                            
                        }

                        if (inputList[i + 1].type == "tree")
                        {
                            int index = FindRoot(inputList[i + 1].tree);
                            inputList[i + 1].tree[index].side = "right";
                            inputList[i + 1].tree[index].SetParent(inputList[i + 1].tree[index], parentNode);
                            for (int r = 0; r < inputList[i + 1].tree.Length; r++)
                            {
                                rightList.Add(inputList[i + 1].tree[r]);
                            }
                        }
                        else
                        {
                            TreeNode rightChild = new TreeNode(inputList[i + 1], parentNode, "right");
                            rightList.Add(rightChild);
                        }

                        //treeNode[] treeNodeArrayInsert = new treeNode[leftList.Count+rightList.Count+1];
                        //int k = 0;
                        //for (int w = 0; w < leftList.Count; w++)
                        //{
                        //    treeNodeArrayInsert[k] = leftList[w];
                        //    k++;
                        //}
                        //for (int w = 0; w < rightList.Count; w++)
                        //{
                        //    treeNodeArrayInsert[k] = rightList[w];
                        //    k++;
                        //}
                        //treeNodeArrayInsert[treeNodeArrayInsert.Length - 1] = parentNode;
                        TreeNode[] treeNodeArrayInsert = new TreeNode[1];
                        treeNodeArrayInsert[0] = parentNode;

                        Token treeNodeToken = new Token();
                        treeNodeToken.type = "tree";
                        treeNodeToken.tree = treeNodeArrayInsert;
                        inputList[i] = treeNodeToken;
                        inputList.RemoveAt(i + 1);
                        inputList.RemoveAt(i - 1);
                        
                    }
                }
            }
            if (inputList.Count != 1)
            {
                throw new Exception("number of tokens does not match number of token types");
            }

            if (noOperation)
            {
                TreeNode singleparentNode = new TreeNode(inputList[0], null, null);
                TreeNode[] singleNodeArrayInsert = new TreeNode[1];
                singleNodeArrayInsert[0] = singleparentNode;
                
                treeNodeArray = singleNodeArrayInsert;
            }
            else
            {
                treeNodeArray = inputList[0].tree;
            }

            return treeNodeArray;
        }

        private int FindRoot(TreeNode[] input)
        {
            if(input == null)
            {
                throw new Exception();
            }
            int index = input.Length - 1;
            while (input[index].parent != null)
            {
                TreeNode temp = input[index].parent;
                for (int n = 0; n < input.Length; n++)
                {
                    if (input[n] == temp)
                    {
                        index = n;
                    }
                }
            }
            return index;
        }

        private Token[] ImplicitNegative(Token[] input)
        {
            List<Token> inputList = ArrayToList(input);
            Token zero = new Token();
            zero.type = "number";
            zero.contents = "0";
            for (int i = 0; i < inputList.Count; i++)
            {
                if(inputList[i].contents == "-" )
                {
                    if(i == 0)
                    {
                        inputList.Insert(0, zero);
                    }
                    else if (inputList[i-1].contents == "(" )
                    {
                        inputList.Insert(i, zero);
                    }
                }
            }

            Token[] output = ListToArray(inputList);
            return output;
        }
        
        private Token[] ListToArray(List<Token> input)
        {
            Token[] output = new Token[input.Count];
            for (int i = 0; i < input.Count; i++)
            {
                output[i] = input[i];
            }
            return output;
        }
        
        private List<Token> ArrayToList(Token[] input)
        {
            List<Token> output = new List<Token>();
            for (int i = 0; i < input.Length; i++)
            {
                output.Add(input[i]);
            }
            return output;
        }

        //private Token[] AddOrder(Token[] input)
        //{
        //    for (int i = 0; i < input.Length; i++)
        //    {
        //        for (int z = 0; z < charOperationArray.Length; z++)
        //        {
        //            string check = string.Empty + charOperationArray[z];
        //            if((string)input[i].contents == check)
        //            {
        //                input[i].order = orderOperationArray[z];
        //            }
        //        }
        //    }
        //    return input;
        //}
        //private int getOperationIndex(char input)
        //{
        //    for (int i = 0; i < charOperationArray.Length; i++)
        //    {
        //        if (input == charOperationArray[i])
        //        {
        //            return i;
        //        }

        //    }
        //    return -1;
        //}
    }

    public class TreeNode
    {
        public Token token;       
        public string? side;
        public TreeNode? parent;
        public TreeNode? leftChild;
        public TreeNode? rightChild;

        public TreeNode(Token inputToken, TreeNode? inputParent, string? inputSide)
        {
            token = inputToken;
            parent = inputParent;
            side = inputSide;
            leftChild = null;
            rightChild = null;

            if(side == "left" && parent != null)
            {
                parent.leftChild = this;
            }
            else if (parent != null)
            {
                parent.rightChild = this;
            }
            

        }

        public void SetParent(TreeNode input, TreeNode inputParent)
        {
            parent = inputParent;
            if (input.side == "left" && parent != null)
            {
                parent.leftChild = this;
            }
            else if (parent != null)
            {
                parent.rightChild = this;
            }
        }




    }


}

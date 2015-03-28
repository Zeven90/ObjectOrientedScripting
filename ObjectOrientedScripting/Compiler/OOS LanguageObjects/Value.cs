﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.OOS_LanguageObjects
{
    class Value : IInstruction
    {
        private IInstruction _parent;
        private string _value;
        private Value(IInstruction parent, string value)
        {
            this._parent = parent;
            this._value = value;
        }
        public static IInstruction parse(IInstruction parent, string toParse)
        {
            //throw new Exception("Identifier '" + name + "' contains not allowed characters for identifierts, allowed characters: regex [_0-9A-Za-z]");
            string value = toParse.Trim();
            if (value.Length < 1)
                throw new Exception("Value is too short for a valid value (length < 1)");
            if (value[0] == '"' || value[0] == '\'')
            {//String
                //Validate the string
                char controlChar = value[0];
                string endString = "";
                int count = 2;
                int backslashCount = 0;
                foreach (char c in value)
                {
                    if (count == 0)
                        break;
                    endString += c;
                    if (c == controlChar)
                        count--;
                    if (c == '\\')
                        backslashCount++;
                    else
                        backslashCount = 0;
                }
                value = endString;
            }
            else if (char.IsNumber(value[0]))
            {//Scalar
                //Validate the scalar
                string endString = "";
                bool comma = false;
                foreach(char c in value)
                {
                    if (!(char.IsNumber(c) || (c == '.' && !comma)))
                        break;
                    endString += c;
                }
                value = endString;
            }
            else if (value[0] == '{')
            {//Array
                //ReadToEnd (LazyArray parsing as we expect the user here that everything is correct)
                //ToDo: change lazy array mode to a proper array mode which also validates the input (lazy mode is shit as we can break code here ... but fine for a prototype)
                int index = value.LastIndexOf('}');
                if(index != value.Length - 1)
                    value = value.Remove(index + 1);
                value = value.Replace('{', '[').Replace('}', ']');
            }
            else
                throw new Exception("Cannot parse unknown value: " + value);
            return new Value(parent, value);
        }
        /**Prints out given instruction into StreamWriter as SQF. writer object is either a string or a StreamWriter*/
        void printInstructions(object writer, bool printTabs = true)
        {
            if (!(writer is System.IO.StreamWriter))
                throw new Exception("printInstruction expected a StreamWriter object but received a " + writer.GetType().Name + " object");
            ((System.IO.StreamWriter)writer).Write((printTabs ? new string('\t', this.getTabs()) : "") + this._value);
        }
        /**Parses given string input specially for this element (example use: foreach(var foo in bar) would replace every occurance of foo with _x and every occurence of _x with __x or something like that)*/
        string parseInput(string input)
        {
            return this._parent.parseInput(input);
        }
        /**returns parent IInstruction which owns this IInstruction (only will return null for the oos namespace object which is the root node for anything)*/
        IInstruction getParent()
        {
            return this._parent;
        }
        /**returns a list of child IInstructions with given type*/
        IInstruction[] getChildInstructions(Type t, bool recursive = true)
        {
            return new IInstruction[] { };
        }
        /**returns first occurance of given type in tree or NULL if nothing was found*/
        IInstruction getFirstOf(Type t)
        {
            IInstruction firstOccurance = this.getParent().getFirstOf(t);
            return (firstOccurance == null ? (this.GetType().Equals(t) ? this : null) : firstOccurance);
        }
        /**Adds given instruction to child instruction list and checks if it is valid to own this instruction*/
        void addInstruction(IInstruction instr)
        {
            throw new Exception("An Identifier cannot have sub isntructions");
        }
        /**returns current tab ammount*/
        int getTabs()
        {
            return this._parent.getTabs();
        }
    }
}
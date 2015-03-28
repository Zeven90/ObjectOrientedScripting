﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.OOS_LanguageObjects
{
    public class Identifier : IInstruction
    {
        private IInstruction _parent;
        private string _name;
        private Identifier(IInstruction parent, string value)
        {
            this._parent = parent;
            this._name = this.parseInput(value);
        }
        private static bool isValidIdentifierChar(char c)
        {
            return char.IsLetterOrDigit(c) || c == '_';
        }
        /**Expects toParse to contain the FULL identifier!*/
        public static IInstruction parse(IInstruction parent, string toParse)
        {
            string name = toParse.Trim();
            if (!name.All(isValidIdentifierChar))
                throw new Exception("Identifier '" + name + "' contains not allowed characters for identifierts, allowed characters: [_0-9A-Za-z]");
            return new Identifier(parent, name);
        }
        /**Prints out given instruction into StreamWriter as SQF. writer object is either a string or a StreamWriter*/
        public void printInstructions(object writer, bool printTabs = true)
        {
            if (!(writer is System.IO.StreamWriter))
                throw new Exception("printInstruction expected a StreamWriter object but received a " + writer.GetType().Name + " object");
            ((System.IO.StreamWriter)writer).Write((printTabs ? new string('\t', this.getTabs()) : "") + this._name);
        }
        /**Parses given string input specially for this element (example use: foreach(var foo in bar) would replace every occurance of foo with _x and every occurence of _x with __x or something like that)*/
        public string parseInput(string input)
        {
            return this.getParent().parseInput(input);
        }
        /**returns parent IInstruction which owns this IInstruction (only will return null for the oos namespace object which is the root node for anything)*/
        public IInstruction getParent()
        {
            return this._parent;
        }
        /**returns a list of child IInstructions with given type*/
        public IInstruction[] getChildInstructions(Type t, bool recursive = true)
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
        public void addInstruction(IInstruction instr)
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
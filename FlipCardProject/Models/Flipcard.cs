namespace FlipCardProject.Models;

using System;
using FlipCardProject.Enums;
using FlipCardProject.Records;

public struct Flipcard
{
    private int _id;
    private string _concept;
    private string _mnemonic;
    private FlipcardState _state;
    private string _question;

    
    public int Id
    {
        get { return _id; }
        set { _id = value; }
    }
    public string Question
        {
            get { return _question; }
            set { _question = value; }
        }
    public string Concept
    {
        get { return _concept; }
        set { _concept = value; }
    }

    public string Mnemonic
    {
        get { return _mnemonic; }
        set { _mnemonic = value; }
    }

    public FlipcardState State
    {
        get { return _state; }
        set { _state = value; }
    }

    public Flipcard(string question,string concept, string mnemonic, FlipcardState state)
    {
        _question = question;
        _concept = concept;
        _mnemonic = mnemonic;
        _state = state;
    }

    public Flipcard()
    {
        _id = -1;
    }
}

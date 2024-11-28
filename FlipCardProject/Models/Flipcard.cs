using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace FlipCardProject.Models;

using System;
using FlipCardProject.Enums;
using FlipCardProject.Records;
[Owned]
public sealed record Flipcard : IEquatable<Flipcard>
{
    
    private int _id;
   
    private string _concept;
    private string _mnemonic;
    //private FlipcardState _state;
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

    /*public FlipcardState State
    {
        get { return _state; }
        set { _state = value; }
    }*/
    

    public Flipcard(string question,string concept, string mnemonic)
    {
        _question = question;
        _concept = concept;
        _mnemonic = mnemonic;
       // _state = state;
      //  UserId = 0;
    
    }

    
    public bool Equals(Flipcard other)
    {
        if (other is null) return false;
        return _mnemonic == other._mnemonic &&
               //_state == other._state &&
               _question == other._question &&
               _concept == other._concept;
    }

    /*public override bool Equals(object? obj)
    {
        return obj is Flipcard card && Equals(card);
    }*/

    public override int GetHashCode()
    {
        return HashCode.Combine(_mnemonic, /*_state,*/ _question, _concept);
    }
}

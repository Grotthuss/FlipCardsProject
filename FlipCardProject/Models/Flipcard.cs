using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace FlipCardProject.Models;

using System;


[Owned]
public sealed record Flipcard : IEquatable<Flipcard>
{
    
    private int _id;
   
    private string _concept;
    private string _mnemonic;
    
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

  
    

    public Flipcard(string question,string concept, string mnemonic)
    {
        _question = question;
        _concept = concept;
        _mnemonic = mnemonic;
       
    
    }

    
    public bool Equals(Flipcard other)
    {
        if (other is null) return false;
        return _mnemonic == other._mnemonic &&
               
               _question == other._question &&
               _concept == other._concept;
    }

   

    public override int GetHashCode()
    {
        return HashCode.Combine(_mnemonic, _question, _concept);
    }
}

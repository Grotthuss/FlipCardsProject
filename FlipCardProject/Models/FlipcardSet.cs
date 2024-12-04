using System.Collections;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace FlipCardProject.Models;

using System;
using System.Collections.Generic;
using Records;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

[JsonObject(MemberSerialization.OptIn)]
[Owned]
public class FlipcardSet : IEquatable<FlipcardSet>
{
    public int Id { get; set; }

    [JsonProperty]
    private string _set_name;

    [JsonProperty]
    private List<Flipcard> _flipcards_list;

    public int UserId { get; set; }

    public string Name
    {
        get { return _set_name; }
        set { _set_name = value; }
    }

    public List<Flipcard> FlipcardsList
    {
        get { return _flipcards_list; }
        set { _flipcards_list = value; }
    }

    public FlipcardSet()
    {
        UserId = 0;
        _set_name = string.Empty;
        _flipcards_list = new List<Flipcard>();
    }

    public FlipcardSet(string setName)
    {
        _set_name = setName;
        _flipcards_list = new List<Flipcard>();
    }

    public FlipcardSet(FlipcardSetDto t)
    {
        _flipcards_list = t.FlipcardsList ?? new List<Flipcard>();
        _set_name = t.SetName;
    }

    public void AddFlipcard(FlipcardState state, string question = "No question", string concept = "No concept", string mnemonic = "No mnemonic")
    {
        Flipcard flipcard = new Flipcard(question: question, concept: concept, mnemonic: mnemonic);
        _flipcards_list.Add(flipcard);
    }
    
    public bool Equals(FlipcardSet? other)
    {
        if (other == null)
        {
            return false;
        }
        
        return Id == other.Id
            && UserId == other.UserId
            && _set_name == other._set_name
            && (_flipcards_list?.Count == other._flipcards_list?.Count); 
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as FlipcardSet);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, UserId, _set_name);
    }
}

public class FlipcardSetDto
{
    public string SetName { get; set; }
    public List<Flipcard> FlipcardsList { get; set; } = new List<Flipcard>();
}

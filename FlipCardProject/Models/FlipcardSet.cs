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
public class FlipcardSet
{
    public int Id { get; set; }

    [JsonProperty]
    
    private string _set_name;
    [JsonProperty]
    
    private List<Flipcard> _flipcards_list;
    
    public int UserId { get; set; }
    //public User User { get; set; }
    
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
       // User = null;
    }
    public FlipcardSet(string setName)
    {
        _set_name = setName;
        _flipcards_list = new List<Flipcard>();
    }

    public FlipcardSet(FlipcardSetDto t)
    {
        _flipcards_list = t.FlipcardsList;
        _set_name = t.SetName;
    }
    public void AddFlipcard(FlipcardState state,string question = "No question" ,string concept = "No concept", string mnemonic = "No mnemonic")
    {

        Flipcard flipcard = new Flipcard(question:question,concept: concept, mnemonic: mnemonic, state: state);
        
        /*object cardId = _flipcards_list.Count + 1;
        flipcard.Id = (int)cardId;*/

        _flipcards_list.Add(flipcard);
    }
}

public class FlipcardSetDto
{
    public string SetName { get; set; }
    public List<Flipcard> FlipcardsList { get; set; } = new List<Flipcard>();
}



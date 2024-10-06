using System.Collections;

namespace FlipCardProject.Models;

using System;
using System.Collections.Generic;
using Records;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

[JsonObject(MemberSerialization.OptIn)]
public class FlipcardSet : IEnumerable<Flipcard>
{
    [JsonProperty]
    private string _set_name;
    [JsonProperty]
    private List<Flipcard> _flipcards_list;
    
    
    public IEnumerator<Flipcard> GetEnumerator()
    {
        return _flipcards_list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    public string SetName
    {
        get { return _set_name; }
        set { _set_name = value; }
    }

    public List<Flipcard> FlipcardsList
    {
        get { return _flipcards_list; }
        set { _flipcards_list = value; }
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
        flipcard.Id = _flipcards_list.Count + 1;
        _flipcards_list.Add(flipcard);
    }
}

public class FlipcardSetDto
{
    public string SetName { get; set; }
    public List<Flipcard> FlipcardsList { get; set; } = new List<Flipcard>();
}



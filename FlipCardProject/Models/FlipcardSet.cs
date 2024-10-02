using System.Collections;

namespace FlipCardProject.Models;

using System;
using System.Collections.Generic;
using Records;

public class FlipcardSet : IEnumerable<Flipcard>
{
    private string _set_name;
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

    public void AddFlipcard(FlipcardState state, string concept = "No concept", string mnemonic = "No mnemonic")
    {

        Flipcard flipcard = new Flipcard(concept: concept, mnemonic: mnemonic, state: state);
        flipcard.Id = _flipcards_list.Count + 1;
        _flipcards_list.Add(flipcard);
    }
}



namespace FlipCardProject.Data;

using FlipCardProject.Models;
using Records;
using System.Text.Json;
using System.IO;

public class Serialization
{
    private FlipcardSet _cardSet { get; set; }
    private string _flipCardType { get; set; }
    public string title { get; set; }
    public List<Mnemonic> mnemonics { get; set; } = new List<Mnemonic>();
    
    public class Mnemonic
    {
        public string mnemonic { get; set; }
        public string concept { get; set; }   
    }
    
    //JsonSerializer.Deserialize needs a parameterless constructor
    public Serialization() {}

    public Serialization(FlipcardSet cardSet, string FlipCardType)
    {
        _cardSet = cardSet;
        _flipCardType = FlipCardType;
    }

    public void LoadData()
    {
        string FileName = Path.Combine("Data", _flipCardType + ".json");
        string Json = File.ReadAllText(FileName);
        
        Serialization serialization = JsonSerializer.Deserialize<Serialization>(Json)!;

        _cardSet.SetName = serialization.title;

        foreach (var mnemonic in serialization.mnemonics)
        {
            FlipcardState state = new FlipcardState();
            _cardSet.AddFlipcard(state ,mnemonic.concept, mnemonic.mnemonic);
        }
    }

    public void saveData(FlipcardSet cardSet)
    {
        Serialization serialization = new Serialization();
        serialization.title = cardSet.SetName;

        foreach (var flipcard in cardSet.FlipcardsList)
        {
            serialization.mnemonics.Add(new Mnemonic
            {
                mnemonic = flipcard.Mnemonic,
                concept = flipcard.Concept
            });
        }
        
        var options = new JsonSerializerOptions();

        options.PropertyNameCaseInsensitive = true;
        options.WriteIndented = true;
        
        string Json = JsonSerializer.Serialize<Serialization>(serialization, options)!;
        
        File.WriteAllText(Path.Combine("Data", serialization.title + ".json"), Json);
    }
}
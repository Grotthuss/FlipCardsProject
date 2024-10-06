using System.Text.Json.Serialization;

namespace FlipCardProject.Data;

using FlipCardProject.Models;
using FlipCardProject.Records;
using System.Text.Json;
using System.IO;

public class Serialization
{
    public string title { get; set; }
    public List<CardAttributes> cardAttributes { get; set; } = new List<CardAttributes>();
    
    public class CardSetCollection
    {
        public List<Serialization> AllCardSets { get; set; } = new List<Serialization>();
    }
    
    public class CardAttributes
    {
        public string question { get; set; }
        public string mnemonic { get; set; }
        public string concept { get; set; }   
    }
    
    public void LoadData(List<FlipcardSet> cardSets)
    {
        string filePath = Path.Combine("Data", "All Card Sets.json");
        string jsonData = File.ReadAllText(filePath);
        
        CardSetCollection cardSetCollection = JsonSerializer.Deserialize<CardSetCollection>(jsonData)!;

        foreach (var Set in cardSetCollection.AllCardSets)
        {
            FlipcardSet cardSet = new FlipcardSet(Set.title);
            
            foreach (var attribute in Set.cardAttributes)
            {
                FlipcardState state = new FlipcardState();
                cardSet.AddFlipcard(state, attribute.question, attribute.concept, attribute.mnemonic);
            }
            
            cardSets.Add(cardSet);
        }
    }
    
    public void saveData(List<FlipcardSet> cardSets)
    {
        CardSetCollection cardSetCollection = new CardSetCollection();

        foreach (var cardSet in cardSets)
        {
            Serialization serializationCardSet = new Serialization();
            
            serializationCardSet.title = cardSet.SetName;
            
            foreach (var card in cardSet.FlipcardsList)
            {
                serializationCardSet.cardAttributes.Add(new CardAttributes
                {
                    question = card.Question,
                    mnemonic = card.Mnemonic,
                    concept = card.Concept
                });
            }
            
            cardSetCollection.AllCardSets.Add(serializationCardSet);
        }
        
        var options = new JsonSerializerOptions();

        options.PropertyNameCaseInsensitive = true;
        options.WriteIndented = true;
        
        string jsonData = JsonSerializer.Serialize(cardSetCollection, options)!;
        
        File.WriteAllText(Path.Combine("Data", "All Card Sets.json"), jsonData);
    }
}
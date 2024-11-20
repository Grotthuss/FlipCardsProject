using FlipCardProject.Models;
using FlipCardProject.Helpers;

public class FlipcardSetValidator : GenericValidator<FlipcardSet>
{
    public FlipcardSetValidator()
    {
        AddRule(set => !string.IsNullOrWhiteSpace(set.Name)); 
        AddRule(set => set.FlipcardsList != null);          
    }
}
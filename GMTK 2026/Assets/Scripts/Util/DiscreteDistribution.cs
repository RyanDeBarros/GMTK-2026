using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiscreteDistribution<T>
{
    public class Choice
    {
        public T value;
        public float weight;
    }

    private readonly List<Choice> choices = new();

    public DiscreteDistribution<T> AddChoice(T value, float weight)
    {
        choices.Add(new()
        {
            value = value,
            weight = weight
        });

        return this;
    }

    public T Poll()
    {
        float totalWeight = choices.Sum(c => c.weight);
        float r = Random.value * totalWeight;

        foreach (Choice choice in choices)
        {
            if (r <= choice.weight)
                return choice.value;
            else
                r -= choice.weight;
        }

        return choices.Last().value;
    }
}

﻿namespace Domain.Abstractions.Classes;

public abstract class Entity
{
    public Guid Id { get; set;  } = Guid.NewGuid();
}


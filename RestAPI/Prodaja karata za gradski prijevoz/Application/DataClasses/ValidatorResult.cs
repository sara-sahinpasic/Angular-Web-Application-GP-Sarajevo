﻿namespace Application.DataClasses;

public class ValidatorResult
{
    public bool IsValid { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
}

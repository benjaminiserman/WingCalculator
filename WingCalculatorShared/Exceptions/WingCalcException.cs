namespace WingCalculatorShared.Exceptions;
using System;

public class WingCalcException : Exception
{
	public WingCalcException() { }
	public WingCalcException(string message) : base(message) { }
	public WingCalcException(string message, Exception innerException) : base(message, innerException) { }
}

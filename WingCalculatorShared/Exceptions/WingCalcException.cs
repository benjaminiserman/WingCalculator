namespace WingCalculatorShared.Exceptions;
using System;

internal class WingCalcException : Exception
{
	public WingCalcException() { }
	public WingCalcException(string message) : base(message) { }
	public WingCalcException(string message, Exception innerException) : base(message, innerException) { }
}

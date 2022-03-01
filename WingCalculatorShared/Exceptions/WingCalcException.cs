namespace WingCalculatorShared.Exceptions;
using System;

public class WingCalcException : Exception
{
	public new string StackTrace { get; private set; }

	public WingCalcException() { }
	internal WingCalcException(string message) : base(message) { }
	internal WingCalcException(string message, Scope scope) : base(message) => StackTrace = scope.Trace();
}

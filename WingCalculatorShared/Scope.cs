namespace WingCalculatorShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal record Scope(LocalList LocalList, Scope ParentScope, Solver Solver) { }
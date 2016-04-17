using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crisp.Shared;

namespace Crisp.Evaluation
{
    public class EvaluatorFactory : IEvaluatorFactory
    {
        private readonly ISpecialFormLoader _specialFormLoader;

        public EvaluatorFactory(ISpecialFormLoader specialFormLoader)
        {
            _specialFormLoader = specialFormLoader;
        }

        public IEvaluator Get()
        {
            var ev = new Evaluator();
            ev.Mutate(_specialFormLoader.GetBindings());
            return ev;
        }
    }
}

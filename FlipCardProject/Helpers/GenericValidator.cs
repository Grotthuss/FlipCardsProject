namespace FlipCardProject.Helpers
{
    public class GenericValidator<T> where T : class, IEquatable<T>
    {
        private readonly List<Func<T, bool>> _rules = new();

        public GenericValidator<T> AddRule(Func<T, bool> rule)
        {
            _rules.Add(rule);
            return this;
        }

        public bool Validate(T entity, out List<string> errors)
        {
            errors = new List<string>();

            foreach (var rule in _rules)
            {
                if (!rule(entity))
                {
                    errors.Add($"Validation failed for {typeof(T).Name}: {rule.Method.Name}");
                }
            }

            return errors.Count == 0;
        }

        public IEnumerable<string> Validate(IEnumerable<T> entities)
        {
            var errors = new List<string>();

            foreach (var entity in entities)
            {
                if (!Validate(entity, out var entityErrors))
                {
                    errors.AddRange(entityErrors);
                }
            }

            return errors;
        }
    }
}
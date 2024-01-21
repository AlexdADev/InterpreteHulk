namespace Hulk
{
    public class FunctionDeclarationNode : Node
    {
        // Un campo que almacena el nombre de la función
        public string Name { get; }

        // Un campo que almacena la lista de parámetros de la función
        public List<string> Parameters { get; }

        // Un campo que almacena el cuerpo de la función
        public Node Body { get; }

        // Un constructor que recibe el nombre de la función, la lista de parámetros y el cuerpo de la función
        public FunctionDeclarationNode(string name, List<string> parameters, Node body)
        {
            this.Name = name;
            this.Parameters = parameters;
            this.Body = body;
        }

        // Implementa el método Evaluate() heredado de la clase Node
        public override object Evaluate()
        {
            // En este método, no se evalúa la función en este momento
            // Se devuelve la propia instancia de la declaración de la función
            return this;
        }

        // Implementa el método EvaluateWithVariables() heredado de la clase Node
        public override object EvaluateWithVariables(Dictionary<string, Node> variables)
        {
            // Se crea un nuevo diccionario para las variables locales de la función
            Dictionary<string, Node> localVariables = new Dictionary<string, Node>(variables);

            // Se agregan los parámetros al diccionario local
            for (int i = 0; i < Parameters.Count; i++)
            {
                localVariables[Parameters[i]] = variables[Parameters[i]];
            }

            // Se evalúa el cuerpo de la función en el contexto local y se devuelve el resultado
            return Body.EvaluateWithVariables(localVariables);
        }
    }

}
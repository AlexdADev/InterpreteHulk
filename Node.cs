namespace Hulk
{
    public abstract class Node
    {
        // Este método es abstracto y debe ser implementado por las clases derivadas.
        // Representa la lógica para evaluar un nodo particular en el árbol sintáctico y devuelve un resultado.
        public abstract object Evaluate();

        // Este método virtual proporciona una forma de evaluar un nodo teniendo en cuenta un diccionario de variables.
        // Por defecto, simplemente llama a Evaluate(), pero las clases derivadas pueden sobrescribirlo para considerar variables.
        public virtual object EvaluateWithVariables(Dictionary<string, Node> variables)
        {
            return Evaluate();
        }
    }

}
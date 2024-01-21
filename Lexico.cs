namespace Hulk
{
    class Lexico
    {
        private string input; //Un campo privado que almacena la cadena de código fuente de entrada.

        public List<Token> tokens;   // Lista para almacenar los tokens encontrados.
        private int posicioActual;  // La posición actual en la cadena de entrada.

        public Lexico(string input)
        {
            this.input = input;           // Inicializa la cadena de código fuente.
            this.tokens = new List<Token>();  // Inicializa la lista de tokens.
            this.posicioActual = 0;     // Inicializa la posición actual en 0.
        }
        // Método principal para dividir la cadena en tokens.
        // Declaramos un método público llamado Tokenize que no toma ningún parámetro.
        public Token[] Tokenizar()
        {
            // Mientras la posición actual sea menor que la longitud de la entrada...
            while (posicioActual < input.Length)
            {
                // ...obtenemos el carácter actual.
                char caracter = input[posicioActual];

                // Si el carácter actual es un espacio en blanco, avanzamos la posición actual y continuamos con la siguiente iteración del bucle.
                if (char.IsWhiteSpace(caracter))
                {
                    posicioActual++;
                    continue;
                }
                // Si el carácter actual es un guión y es un operador unario, escaneamos el operador unario, lo añadimos a la lista de tokens y continuamos con la siguiente iteración del bucle.
                else if ((caracter == '-' && IsUnaryOperator()))
                {
                    Token token = ScanUnaryOperator();
                    tokens.Add(token);
                }
                // Si el carácter actual es 'c' y la palabra clave "cos" coincide, creamos un nuevo token con el tipo TokenType.CosFunction y el valor "cos", lo añadimos a la lista de tokens, avanzamos la posición actual en 3 para saltar "cos" y continuamos con la siguiente iteración del bucle.
                else if (caracter == 'c' && MatchKeyword("cos"))
                {
                    Token token = new Token(TokenType.CosFunction, "cos");
                    tokens.Add(token);
                    posicioActual += 3; // Saltar "cos"
                }
                // Si el carácter actual es 's' y la palabra clave "sen" coincide, creamos un nuevo token con el tipo TokenType.SenFunction y el valor "sen", lo añadimos a la lista de tokens, avanzamos la posición actual en 3 para saltar "sen" y continuamos con la siguiente iteración del bucle.
                else if (caracter == 's' && MatchKeyword("sen"))
                {
                    Token token = new Token(TokenType.SenFunction, "sen");
                    tokens.Add(token);
                    posicioActual += 3; // Saltar "sen"
                }
                // Si el carácter actual es 'l' y la palabra clave "log" coincide, creamos un nuevo token con el tipo TokenType.LogFunction y el valor "log", lo añadimos a la lista de tokens, avanzamos la posición actual en 3 para saltar "log" y continuamos con la siguiente iteración del bucle.
                else if (caracter == 'l' && MatchKeyword("log"))
                {
                    Token token = new Token(TokenType.LogFunction, "log");
                    tokens.Add(token);
                    posicioActual += 3; // Saltar "log"
                }
                // Si el carácter actual es un dígito, escaneamos el número, lo añadimos a la lista de tokens y continuamos con la siguiente iteración del bucle.
                else if (char.IsDigit(caracter))
                {
                    Token token = ScanNumber();
                    tokens.Add(token);
                }
                // Si el carácter actual es una letra, escaneamos el identificador o la palabra clave, lo añadimos a la lista de tokens y continuamos con la siguiente iteración del bucle.
                else if (char.IsLetter(caracter))
                {
                    Token token = ScanIdentifierOrKeyword();
                    tokens.Add(token);
                }
                // Si el carácter actual es un igual y coincide con una flecha, creamos un nuevo token con el tipo TokenType.Arrow y el valor "=>", lo añadimos a la lista de tokens, avanzamos la posición actual en 2 para saltar "=>" y continuamos con la siguiente iteración del bucle.
                else if (caracter == '=' && MatchArrow())
                {
                    Token token = new Token(TokenType.Arrow, "=>");
                    tokens.Add(token);
                    posicioActual += 2; // Saltar "=>"
                }
                // Si el carácter actual es un operador, escaneamos el operador, creamos un nuevo token con el tipo TokenType.Operator y el valor del operador, lo añadimos a la lista de tokens y continuamos con la siguiente iteración del bucle.
                else if (IsOperator(caracter))
                {
                    string token = ScanOperator();
                    tokens.Add(new Token(TokenType.Operator, token));
                }
                // Si el carácter actual es una paréntesis, creamos un nuevo token con el tipo TokenType.Parenthesis y el valor del carácter actual, lo añadimos a la lista de tokens, avanzamos la posición actual y continuamos con la siguiente iteración del bucle.
                else if (caracter == '(' || caracter == ')')
                {
                    tokens.Add(new Token(TokenType.Parenthesis, caracter.ToString()));
                    posicioActual++;
                }
                // Si el carácter actual es un punto y coma, creamos un nuevo token con el tipo TokenType.PuntoYComa y el valor del carácter actual, lo añadimos a la lista de tokens, avanzamos la posición actual y continuamos con la siguiente iteración del bucle.
                else if (caracter == ';')
                {
                    tokens.Add(new Token(TokenType.PuntoYComa, caracter.ToString()));
                    posicioActual++;
                }
                // Si el carácter actual es una coma, creamos un nuevo token con el tipo TokenType.Coma y el valor del carácter actual, lo añadimos a la lista de tokens, avanzamos la posición actual y continuamos con la siguiente iteración del bucle.
                else if (caracter == ',')
                {
                    tokens.Add(new Token(TokenType.Coma, caracter.ToString()));
                    posicioActual++;
                }
                // Si el carácter actual es una comilla doble, escaneamos la cadena, la añadimos a la lista de tokens y continuamos con la siguiente iteración del bucle.
                else if (caracter == '"')
                {
                    Token token = ScanString();
                    tokens.Add(token);
                }
                // Si el carácter actual no es ninguno de los anteriores, lanzamos una excepción indicando un carácter no válido.
                else
                {
                    throw new Exception($"Carácter no válido: {caracter}");
                }
            }

            // Si la entrada termina con un punto y coma, devolvemos la lista de tokens como un array.
            if (input.TrimEnd().EndsWith(";"))
            {
                return tokens.ToArray();
            }
            // Si la entrada no termina con un punto y coma, lanzamos una excepción indicando un error de sintaxis.
            else
            {
                throw new Exception("!SYNTAX ERROR: La entrada debe terminar con un punto y coma (;).");
            }
        }
        // Esta función verifica si el próximo carácter en la entrada es un operador unario
        private bool IsUnaryOperator()
        {
            // Calcula la posición del próximo carácter
            int nextPosition = posicioActual + 1;
            // Si la próxima posición es válida y el carácter en esa posición es un dígito
            if (nextPosition < input.Length && char.IsDigit(input[nextPosition]))
            {
                // Entonces, el '-' es un operador unario, por lo que devuelve verdadero
                return true;
            }
            // Si no, devuelve falso
            return false;
        }

        // Esta función escanea la entrada en busca de un operador unario
        private Token ScanUnaryOperator()
        {
            // Guarda la posición actual para poder revertir si es necesario
            int startPosition = posicioActual;
            // Salta los espacios en blanco
            while (posicioActual < input.Length && char.IsWhiteSpace(input[posicioActual]))
            {
                posicioActual++;
            }

            // Si el carácter actual es un '-', entonces es un operador unario
            if (posicioActual < input.Length && input[posicioActual] == '-')
            {
                // Avanza a la próxima posición
                posicioActual++;
                // Devuelve un nuevo token que representa el operador unario
                return new Token(TokenType.UnaryOperator, "-");
            }

            // Si no se encontró un operador unario, revierte la posición actual a la posición de inicio
            posicioActual = startPosition;
            // Y devuelve null
            return null;
        }

        // Declaramos un método privado llamado MatchKeyword que toma un string llamado keyword como parámetro.
        private bool MatchKeyword(string keyword)
        {
            // Obtenemos la longitud de la palabra clave y la almacenamos en la variable keywordLength.
            int keywordLength = keyword.Length;

            // Verificamos si la posición actual más la longitud de la palabra clave es menor o igual a la longitud de la entrada.
            // Esto es para asegurarnos de que no nos salgamos de los límites de la cadena de entrada.
            if (posicioActual + keywordLength <= input.Length)
            {
                // Si la condición anterior es verdadera, entonces extraemos una subcadena de la entrada que comienza en la posición actual y tiene la longitud de la palabra clave.
                // Almacenamos esta subcadena en la variable potentialKeyword.
                string potentialKeyword = input.Substring(posicioActual, keywordLength);

                // Comparamos la subcadena extraída con la palabra clave. Si son iguales, entonces la función devuelve true.
                return potentialKeyword == keyword;
            }

            // Si la condición del if no se cumple (es decir, la posición actual más la longitud de la palabra clave es mayor que la longitud de la entrada), entonces la función devuelve false.
            return false;
        }
        // Método para escanear y reconocer un número.
        private Token ScanNumber()
        {
            // Inicializamos una cadena vacía para almacenar el número.
            string number = "";

            // Mientras la posición actual sea menor que la longitud de la entrada y el carácter actual sea un dígito o un punto...
            while (posicioActual < input.Length && (char.IsDigit(input[posicioActual]) || input[posicioActual] == '.'))
            {
                // ...añadimos el carácter actual a la cadena del número y avanzamos la posición actual.
                number += input[posicioActual];
                posicioActual++;
            }

            // Si la posición actual es menor que la longitud de la entrada y el carácter actual es una letra o un dígito...
            if (posicioActual < input.Length && (char.IsLetter(input[posicioActual]) || char.IsDigit(input[posicioActual])))
            {
                // ...entonces tenemos un error. Inicializamos la posición del error a la posición actual.
                int errorPosition = posicioActual;

                // Mientras la posición del error sea menor que la longitud de la entrada y el carácter en la posición del error sea una letra o un dígito...
                while (errorPosition < input.Length && (char.IsLetter(input[errorPosition]) || char.IsDigit(input[errorPosition])))
                {
                    // ...avanzamos la posición del error.
                    errorPosition++;
                }

                // Extraemos el token inválido de la entrada y lanzamos una excepción.
                string invalidToken = input.Substring(posicioActual - 1, errorPosition - posicioActual + 1);
                throw new Exception($"!LEXICAL ERROR: {invalidToken} no es un token válido");
            }

            // Si el número contiene un punto...
            if (number.Contains("."))
            {
                // ...intentamos convertirlo a un número decimal. Si tiene éxito, devolvemos el valor decimal como un número.
                // Si no, lanzamos una excepción.
                if (double.TryParse(number, out double decimalValue))
                {
                    return new Token(TokenType.Number, decimalValue.ToString()); // Devolvemos el valor decimal como un número
                }
                else
                {
                    throw new Exception($"Número no válido: {number}");
                }
            }
            else
            {
                // Si el número no contiene un punto, intentamos convertirlo a un número entero. Si tiene éxito, devolvemos el valor entero como un número.
                // Si no, lanzamos una excepción.
                if (int.TryParse(number, out int intValue))
                {
                    return new Token(TokenType.Number, intValue.ToString()); // Devolvemos el valor entero como un número
                }
                else
                {
                    throw new Exception($"Número no válido: {number}");
                }
            }
        }
        // Método para escanear y reconocer un identificador o palabra clave.
        private Token ScanIdentifierOrKeyword()
        {
            // Inicializamos una cadena vacía para almacenar el identificador.
            string identifier = "";

            // Mientras la posición actual sea menor que la longitud de la entrada y el carácter actual sea una letra, un dígito o un guión bajo...
            while (posicioActual < input.Length && (char.IsLetterOrDigit(input[posicioActual]) || input[posicioActual] == '_'))
            {
                // ...añadimos el carácter actual a la cadena del identificador y avanzamos la posición actual.
                identifier += input[posicioActual];
                posicioActual++;
            }

            // Si el identificador no es válido...
            if (!IsIdentifierValid(identifier))
            {
                // ...lanzamos una excepción indicando un error léxico.
                throw new Exception($"LEXICAL ERROR: Identificador no válido: {identifier}");
            }

            // Si el identificador es "PI", devolvemos un nuevo token con el tipo TokenType.PI y el valor de PI.
            if (identifier == "PI")
            {
                return new Token(TokenType.PI, Math.PI.ToString());
            }

            // Verificamos si el identificador es una palabra clave y, de ser así, devolvemos un nuevo token con el tipo correspondiente y el identificador como valor.
            if (identifier == "if")
            {
                return new Token(TokenType.If, identifier);
            }
            else if (identifier == "else")
            {
                return new Token(TokenType.Else, identifier);
            }
            else if (identifier == "let")
            {
                return new Token(TokenType.Let, identifier);
            }
            else if (identifier == "in")
            {
                return new Token(TokenType.In, identifier);
            }
            else if (identifier == "print")
            {
                return new Token(TokenType.Print, identifier);
            }
            else if (identifier == "function") // Nueva palabra clave 'function'
            {
                return new Token(TokenType.FunctionDeclaration, identifier);
            }

            // Si el identificador no es ninguna de las palabras clave anteriores, devolvemos un nuevo token con el tipo TokenType.Identifier y el identificador como valor.
            return new Token(TokenType.Identifier, identifier);
        }


        private bool IsIdentifierValid(string identifier)
        {
            // Verificar si el identificador contiene solo letras, dígitos o guiones bajos
            return System.Text.RegularExpressions.Regex.IsMatch(identifier, "^[a-zA-Z0-9_]+$");
        }
        private bool MatchArrow()
        {
            int nextPosition = posicioActual + 1;
            if (nextPosition < input.Length && input[nextPosition] == '>')
            {
                return true;
            }
            return false;
        }
        private bool IsOperator(char c)
        {
            char[] validOperators = { '+', '-', '*', '/', '^', '=', '<', '>', '!', '@', '%' };
            return Array.IndexOf(validOperators, c) != -1;
        }
        // Declaramos un método privado llamado ScanOperator que no toma ningún parámetro.
        private string ScanOperator()
        {
            // Inicializamos una cadena vacía para almacenar el valor del operador.
            string operatorValue = "";

            // Mientras la posición actual sea menor que la longitud de la entrada y el carácter actual sea un operador...
            while (posicioActual < input.Length && IsOperator(input[posicioActual]))
            {
                // ...añadimos el carácter actual a la cadena del operador y avanzamos la posición actual.
                operatorValue += input[posicioActual];
                posicioActual++;
            }

            // Devolvemos el valor del operador.
            return operatorValue;
        }
        // Declaramos un método privado llamado ScanString que no toma ningún parámetro.
        private Token ScanString()
        {
            // Avanzamos la posición actual para saltar la comilla doble inicial de la cadena.
            posicioActual++;

            // Inicializamos una cadena vacía para almacenar el valor de la cadena.
            string value = "";

            // Mientras la posición actual sea menor que la longitud de la entrada...
            while (posicioActual < input.Length)
            {
                // ...obtenemos el carácter actual.
                char currentChar = input[posicioActual];

                // Si el carácter actual es una comilla doble...
                if (currentChar == '"')
                {
                    // ...avanzamos la posición actual para saltar la comilla doble final de la cadena y devolvemos un nuevo token con el tipo TokenType.String y el valor de la cadena.
                    posicioActual++;
                    return new Token(TokenType.String, value);
                }

                // Si el carácter actual no es una comilla doble, lo añadimos al valor de la cadena y avanzamos la posición actual.
                value += currentChar;
                posicioActual++;
            }

            // Si hemos llegado al final de la entrada y no hemos encontrado una comilla doble final para la cadena, lanzamos una excepción indicando un error de sintaxis.
            throw new Exception("!SYNTAX ERROR: Falta una comilla doble de cierre.");
        }


    }

}
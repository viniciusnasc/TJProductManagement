## Projeto para vaga de desenvolvedor junior - Gerenciamento de produto  

Este teste consiste em realizar dois microserviços, um contendo um CRUD simples de produto e o outro um serviço de log.  

### Requisitos obrigatórios:
- MongoDB: A persistencia de dados deve ser realizada com o banco MongoDB;
- Redis: O projeto deve utilizar serviço de cache para operações de leitura;
- Amazon SQS: A comunicação entre os projetos deve ser realizado via o serviço de filas da amazon;
- LocalStack: Necessário para rodar a instancia do SQS localmente;  
- Docker: O MongoDB e o Redis como requisito devem ser utilizados via docker;  

### Observações:
- Para o CRUD, optei por utilizar um projeto ASP.NET Core Web API separando-o em camadas para melhor avaliação.  
- Para o serviços de log, optei por utilizar uma Console App por ser um serviço mais simples.  
  
### Links utilizados:

https://aws.amazon.com/pt/cli/ => Download da cli da AWS;  
https://docs.localstack.cloud/getting-started/installation/ => Instalação da Cli do localStack;  
https://docs.localstack.cloud/user-guide/integrations/aws-cli/#localstack-aws-cli-awslocal;  
https://www.python.org/downloads/windows/  => Necessário para instalação do localStack;  

### Sugestao de melhoria:
- Criar um service para utilizar o redis para não ser necessário instanciá-lo em todos os metodos.  

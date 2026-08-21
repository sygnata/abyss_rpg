# AGENTS.md

## Projeto

AbyssRpg e uma API ASP.NET Core para um RPG, com foco inicial em personagens, progressao, disciplinas e atividades temporizadas.

A solucao segue uma separacao em camadas:

- `AbyssRpg.Api`: endpoints HTTP, contratos de request/response e configuracao da API.
- `AbyssRpg.Application`: casos de uso, comandos, queries, handlers e abstracoes.
- `AbyssRpg.Domain`: regras de negocio puras, entidades, value objects e excecoes de dominio.
- `AbyssRpg.Infrastructure`: persistencia, Entity Framework Core, PostgreSQL e migrations.
- `AbyssRpg.Tests`: testes automatizados.

## Regras de arquitetura

- Nao colocar regra de negocio em controllers.
- Controllers devem apenas receber request, chamar handlers/use cases e retornar response HTTP.
- Regras de progressao, vida, disciplinas e atividades devem ficar no `Domain`.
- Casos de uso e orquestracao devem ficar no `Application`.
- Codigo de banco, EF Core, migrations e repositorios concretos devem ficar no `Infrastructure`.
- A API nao deve depender diretamente de detalhes de infraestrutura alem da configuracao via DI.
- Evitar expor tipos internos da camada `Application` diretamente como contrato publico da API quando o endpoint crescer em complexidade.

## Convencoes de API

- Usar controllers com `[ApiController]`.
- Preferir retornos tipados como `ActionResult<TResponse>` quando possivel.
- Requests e responses HTTP devem ficar na camada `Api`.
- Usar `CancellationToken` em endpoints e handlers.
- Usar `CreatedAtAction` ou `CreatedAtRoute` em endpoints de criacao.
- Documentar respostas com `[ProducesResponseType]` quando adicionar ou alterar endpoints.
- Erros de dominio devem continuar passando pelo tratamento global de excecoes.

## Convencoes de dominio

- O dominio deve ser independente de ASP.NET, EF Core e bibliotecas externas de infraestrutura.
- Metodos de entidades devem preservar invariantes.
- Validacoes criticas devem estar no dominio, mesmo que tambem existam validacoes na API.
- Preferir nomes explicitos para regras de RPG, como XP necessario, ganho de vida, duracao de atividade e multiplicadores.

## Persistencia

- Usar EF Core com PostgreSQL.
- Alteracoes em entidades persistidas provavelmente exigem atualizacao de mapeamento e migration.
- Nao alterar migrations antigas sem motivo forte; criar nova migration para mudancas de schema.
- Repositorios concretos ficam em `Infrastructure`.

## Testes

Antes de finalizar alteracoes relevantes, rodar:

```bash
dotnet test
```

Para mudancas de dominio, adicionar ou ajustar testes unitarios cobrindo regras de negocio.

Para mudancas em endpoints, cobrir pelo menos:

- status HTTP esperado;
- contrato de request/response;
- comportamento de erro relevante.

## Estilo de codigo

- Manter o padrao atual do projeto.
- Preferir codigo simples e explicito.
- Nao introduzir bibliotecas novas sem necessidade clara.
- Nao adicionar abstracoes genericas antes de existir repeticao real.
- Manter nullable reference types respeitados.
- Usar nomes em ingles no codigo.

## Seguranca de mudancas

- Nao fazer refatoracoes amplas junto com mudanca funcional pequena.
- Nao mover classes entre camadas sem justificar a dependencia arquitetural.
- Nao alterar contratos publicos da API sem considerar compatibilidade.
- Se houver duvida entre simplicidade e abstracao, preferir simplicidade.

## Comunicacao de alteracoes

- Sempre que alterar ou adicionar arquivos, informar no resumo final o caminho de cada arquivo criado ou modificado.
- Separar arquivos criados de arquivos alterados quando houver ambos.
- Informar tambem quando nenhuma verificacao automatizada foi executada.

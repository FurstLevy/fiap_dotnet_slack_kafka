# O que é

=====================

Projeto para trabalho de graduação. Worker que irá consumir um tópico do Kafka e enviar para um canal do Slack.

## Tecnologias

- .NET Core 3
- Kafka (confluentinc)

## Requisitos de dev

- VS ou VS Code
- .NET Core SDK 3
- Docker (com docker compose)

## Como executar

- Na pasta raiz, executar:

```bash
docker-compose up
```

- Enviar mensagens para o tópico e acompanhar os logs do consumer.

## Para desenvolver

- Subir um compose apenas com as imagens do kafka e zookeeper. No launchSettings já contém as variáveis de ambiente necessárias.
- Na pasta raiz, executar:

```bash
docker-compose -f docker-compose.development.yml up
```

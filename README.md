# 🧩 MonitorBoard K8S

O **MonitorBoard K8S** é uma prova de conceito que demonstra um ecossistema **event-driven** (baseado em eventos) executando em um ambiente **Kubernetes**.  
O sistema é composto por três projetos principais — uma **API de dashboard**, um **worker de monitoramento** e um **conjunto de manifests Kubernetes** para orquestração dos serviços.

---

## 📁 Estrutura do Repositório

| Projeto | Tipo | Descrição |
|----------|------|------------|
| **BoardsK8s** | API | Dashboard responsável por exibir informações dos objetos do cluster Kubernetes (neste caso, apenas Pods). |
| **MonitorK8sPod** | Worker Service | Serviço responsável por escutar eventos de criação de Pods no cluster e publicar notificações via RabbitMQ e Webhook para a API BoardsK8s. |
| **ManifestsK8s** | Infraestrutura | Conjunto de arquivos manifesto para provisionar os recursos e serviços no cluster Kubernetes. |

---

## 🔃 Fluxo Geral

O fluxo é o seguinte:

- O MonitorK8sPod monitora o cluster via KubernetesClient.
- Ao detectar a criação de um novo Pod, ele publica o evento no RabbitMQ e envia uma notificação Webhook para a BoardsK8s API
- A BoardsK8s API atualiza o dashboard em tempo real via SignalR.
- O ManifestsK8s contém todos os manifestos necessários para implantar e configurar esses componentes no Kubernetes.

---

## ⚙️ Projetos em Detalhe

🧱 **BoardsK8s (API)**

API de dashboard que carrega informações dos objetos do cluster Kubernetes, atualmente focada nos Pods.

**Arquitetura utilizada:**
- Vertical Slice Architecture

**Principais tecnologias:**
- KubernetesClient
- Carter
- FluentValidation
- Mapster
- MediatR
- SignalR
- Scalar

---

## ⚡ MonitorK8sPod (Worker Service)

Serviço responsável por observar o cluster Kubernetes e reagir a eventos de criação de Pods.
Quando novos Pods são detectados, ele envia os eventos para o RabbitMQ e notifica a BoardsK8s API via Webhook, compondo o fluxo event-driven da aplicação.

**Arquitetura utilizada:**
- Arquitetura Limpa
- Arquitetura Cebola (Onion Architecture)
- Arquitetura de Sistema Event-Driven

**Principais tecnologias:**
- Mapster
- MediatR
- KubernetesClient
- RabbitMQ
- Refit
- Polly

---

## 🧾 ManifestsK8s (Infraestrutura)

Projeto responsável pelos arquivos manifesto Kubernetes, que definem a configuração dos serviços e recursos do cluster.

Inclui:

- Deployments e Services para os componentes
- Configurações de RabbitMQ e Redis
- Namespaces e variáveis de ambiente

---

## 🚀 Tecnologias Globais

- .NET 9
- RabbitMQ
- Docker
- Kubernetes

---

## 🧭 Execução Local

Cada projeto possui instruções detalhadas em seu próprio **README**:

- [BoardsK8s](https://github.com/fspelling/MonitorBoardK8s/tree/main/src/BoardK8s)
- [MonitorK8sPod](https://github.com/fspelling/MonitorBoardK8s/tree/main/src/MonitorK8sPod)
- [ManifestsK8s](https://github.com/fspelling/MonitorBoardK8s/tree/main/src/ManifestsK8s)

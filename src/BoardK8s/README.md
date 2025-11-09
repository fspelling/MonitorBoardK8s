# 🧱 BoardsK8s — API de Dashboard do Cluster Kubernetes

O **BoardsK8s** é uma API responsável por fornecer um **dashboard em tempo real** com informações dos objetos do cluster Kubernetes — atualmente focada nos **Pods**.  
Ela atua como o ponto central de visualização e integração do ecossistema **MonitorBoard K8s**, recebendo notificações do worker **MonitorK8sPod** e exibindo os dados via **SignalR**.

---

## 🧩 Papel na Arquitetura

- O MonitorK8sPod detecta novos Pods no cluster e envia uma notificacao para uma fila do RabbitMq e um Webhook para a BoardsK8s API.
- A API processa os dados e atualiza o dashboard em tempo real para todos os clientes conectados via SignalR.

---

## 🧠 Arquitetura Utilizada

O projeto segue o padrão Vertical Slice Architecture, que organiza o código por feature em vez de camadas tradicionais.
Essa abordagem favorece alta coesão, baixo acoplamento e facilita a evolução independente das funcionalidades.

**Benefícios da arquitetura**

- Cada funcionalidade é isolada e autônoma.
- Facilita testes e manutenção.
- Elimina dependências circulares entre camadas.
- Melhora a clareza e a escalabilidade do código

---

## ⚙️ Tecnologias Principais

| Tecnologia | Propósito |
|----------------|---------------|
| **KubernetesClient** | Comunicação com o cluster Kubernetes para leitura de informações dos Pods |
| **Carter** | Simplifica a criação de endpoints HTTP com uma sintaxe minimalista e modular. |
| **FluentValidation** | Validação declarativa de dados de entrada nas rotas. |
| **Mapster** | Mapeamento rápido e eficiente entre DTOs e entidades. |
| **MediatR** | Implementação do padrão Mediator, promovendo desacoplamento entre camadas. |
| **SignalR** | Comunicação em tempo real com o frontend do dashboard. |
| **Scalar** | Ferramenta de documentação e visualização de endpoints da API. |

---

## 🧾 Estrutura do Projeto

```
Poc.BoardK8sApi/
├── Entities/
│   ├── Container.cs
│   ├── Pod.cs
├── Features/
│   ├── Pod/
│   │   ├── GetPods.cs
│   │   ├── NotificationPod.cs
├── Infra/
│   ├── KubernetesClient.cs
├── Mappers/
│   ├── MapperConfig.cs
│   ├── PodMapper.cs
├── Shared/
│   ├── Result.cs
```

---

## ▶️ Executando Localmente

**Pré-requisitos**

- NET 9 SDK
- Docker (para rrodar o cluster localmente)
- Kubernetes cluster configurado (ex: kind ou minikube)

**Passos**

- Clone o repositório:

```bash
git clone https://github.com/seuusuario/MonitorBoardK8s.git
cd MonitorBoardK8s/BoardsK8s
```

- Execute a aplicação:

```bash
dotnet run
```

- Acesse a API:

```bash
http://localhost:7171
```

- (Opcional) Visualize a documentação interativa gerada pelo Scalar:

```bash
http://localhost:7171/scalar
```

---

## 🔄 Comunicação em Tempo Real

A API utiliza SignalR para atualizar o dashboard assim que novos Pods são detectados no cluster.
Os clientes conectados recebem as informações automaticamente, sem necessidade de refresh manual.

---

## 🔒 Validação e Mapeamento

- FluentValidation garante que os dados de entrada estejam consistentes e válidos.
- Mapster realiza o mapeamento entre entidades e DTOs, reduzindo código repetitivo.

---

## 🧰 Futuras Extensões

- Suporte a outros tipos de objetos do Kubernetes (Deployments, Services etc).
- Métricas em tempo real via Prometheus/Grafana.
- Persistência de histórico de Pods.
- Dashboard visual em uma tecnologia Frontend.
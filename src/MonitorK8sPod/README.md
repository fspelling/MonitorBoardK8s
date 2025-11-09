# ⚡ MonitorK8sPod — Worker de Monitoramento de Pods no Cluster Kubernetes

O **MonitorK8sPod** é um **Worker Service** responsável por observar o cluster Kubernetes e reagir a eventos de **criação de Pods**.  
Ao detectar um novo Pod, o serviço **publica eventos no RabbitMQ** e **notifica a API BoardsK8s via Webhook**, alimentando o fluxo **event-driven** do ecossistema **MonitorBoard K8s**.

---

## 🧩 Papel na Arquitetura

- O MonitorK8sPod atua como o “observador” do cluster.
- Quando um novo Pod é criado, ele gera e propaga o evento para os demais componentes.
- A arquitetura é baseada em eventos (event-driven), promovendo baixo acoplamento e alta escalabilidade.

---

### **🧠 Arquitetura Utilizada**

O projeto combina três abordagens arquiteturais complementares:

### **🧱 Arquitetura Limpa (Clean Architecture)**

Organiza o código em camadas independentes, separando regras de negócio da infraestrutura.

### **🧅 Arquitetura Cebola (Onion Architecture)**

Baseia-se em camadas concêntricas, garantindo que as dependências apontem sempre para o domínio (núcleo).

### **⚙️ Arquitetura de Sistema Event-Driven**

Os componentes se comunicam por eventos assíncronos via RabbitMQ e Webhooks, tornando o sistema mais resiliente e desacoplado.

---

## ⚙️ Tecnologias Principais

| Tecnologia | Propósito |
|----------------|---------------|
| **KubernetesClient** | Monitora o cluster Kubernetes em tempo real para capturar eventos de Pods. |
| **RabbitMQ** | Canal de mensageria utilizado para publicação dos eventos de novos Pods. |
| **Refit** | Realiza chamadas HTTP tipadas para envio dos Webhooks à API BoardsK8s. |
| **Polly** | Fornece políticas de resiliência (retries, circuit breakers, timeouts). |
| **Mapster** | Simplifica o mapeamento entre modelos de domínio e DTOs. |
| **MediatR** | Implementa o padrão Mediator, desacoplando as operações internas. |

---

## 🧾 Estrutura do Projeto

```
Core/
├── Application/
│   ├── Poc.MonitorK8sPod.Application/
│   │   ├── Commands/
│   │   ├── Events/Handlers
├── Domain/
│   ├── Poc.MonitorK8sPod.Domain/
│   │   ├── Entities/
│   │   ├── Events/
│   │   ├── Interfaces/
├── Shared/
│   ├── Poc.MonitorK8sPod.Shared/
│   │   ├── Results.cs

External/
├── Worker/
│   ├── Poc.MonitorK8sPod.Worker/
│   │   ├── Config/
│   │   ├── Worker.cs
├── Infra/
│   ├── Poc.MonitorK8sPod.Infra/
│   │   ├── ExternalServices/
│   │   │   ├── Apis/
│   │   │   ├── Factory/
│   │   ├── Kubernetes/
│   │   │   ├── K8sPodWatcher.cs
│   │   │   ├── KubernetesClient.cs
│   │   ├── Messaging/
│   │   │   ├── RabbitmqProducer.cs
│   ├── Poc.MonitorK8sPod.Ioc/
│   │   ├── InjectDependency.cs
│   │   ├── MapperObjects.cs
│   │   ├── RefitClientResilience.cs
```

---

## 🚀 Fluxo de Execução

- O serviço se conecta ao cluster Kubernetes usando KubernetesClient.
- Ao detectar um novo Pod, ele dispara um evento interno com o MediatR.
- O evento é tratado e:
- Publicado no RabbitMQ para consumo assíncrono.
- Enviado via Webhook para a BoardsK8s API.
- Caso ocorram falhas de rede ou indisponibilidade temporária, o Polly aplica políticas de retry e circuit breaker automaticamente.

---

## ▶️ Executando Localmente

**Pré-requisitos**

- .NET 9 SDK
- Acesso a um cluster Kubernetes (ex: minikube ou kind)
- Instância RabbitMQ (local ou container)
- API BoardsK8s em execução (para testar o webhook)

**Passos**

- Clone o repositório:

```bash
git clone https://github.com/seuusuario/MonitorBoardK8s.git
cd MonitorBoardK8s/MonitorK8sPod
```

- Configure as variáveis de ambiente:

```bash
export RabbitMq__Host=amqp://guest:guest@localhost:5672
export Webhook__Url=http://localhost:5000/api/webhook/pods
```

- Execute o worker:

```bash
dotnet run
```

---

## 🧰 Resiliência e Comunicação

O MonitorK8sPod utiliza Refit + Polly para comunicação HTTP resiliente.
Cada tentativa de envio de Webhook é protegida por políticas de:

- Retry com backoff exponencial;
- Timeout configurável;
- Circuit breaker para falhas consecutivas.

---

## 🔔 Eventos Publicados

| Evento | Destino | Descrição |
|-------------------|---------------------|------------------------------------|
| **PodCreatedEvent** | RabbitMQ / Webhook | Disparado quando um novo Pod é criado no cluster. |

Os consumidores podem se inscrever na fila RabbitMQ correspondente para processar os eventos de criação de Pods de forma assíncrona.

---

## 🧱 Benefícios Arquiteturais

- Alta desacoplagem entre produtor e consumidor.
- Reatividade imediata a mudanças no cluster.
- Resiliência em cenários de falha temporária.
- Fácil extensão para outros eventos (como exclusão de Pods, Deployments etc).

---

## 🧩 Integração com o Ecossistema

- Publica eventos que alimentam a BoardsK8s API.
- Pode ser implantado junto ao cluster via ManifestsK8s.
- Comunicação assíncrona garante isolamento e tolerância a falhas.
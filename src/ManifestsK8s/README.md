# 🧾 ManifestsK8s — Manifests do Cluster Kubernetes

O **ManifestsK8s** é o projeto responsável por armazenar e versionar todos os **arquivos manifesto Kubernetes** que definem a infraestrutura e orquestram os serviços do ecossistema **MonitorBoard K8s**.  
Esses manifests permitem implantar, configurar e conectar os componentes do sistema — **BoardsK8s (API)**, **MonitorK8sPod (Worker)** e os serviços auxiliares como **RabbitMQ** — dentro de um mesmo cluster Kubernetes.

## **📋 Pré-requisitos**

- [Docker](https://docs.docker.com/get-docker/)
- [Kind](https://kind.sigs.k8s.io/)
- [Kubectl](https://kubernetes.io/docs/tasks/tools/)

## **📂 Estrutura do projeto**

```text
ManifestsK8s/
├─ config/          
│  ├─ kind-config.yaml
│
├─ boards-k8s/  
│  ├─ deploy/
│  ├─ service-account/
│
└─ monitor-K8s-pod/              
│  ├─ deploy/
│  ├─ service-account/
```

## **🌐 Criação do cluster**

### Criar cluster com Kind:
```bash
kind create cluster --name cluster-k8s --config ./config/kind-config.yaml
```

## **🧩 Deploy Aplicações**

### boards-k8s
```bash
kubectl apply -f ./boards-k8s/service-account/service-account.yaml
kubectl apply -f ./boards-k8s/service-account/role.yaml
kubectl apply -f ./boards-k8s/service-account/role-binding.yaml
kubectl apply -f ./boards-k8s/deploy/configmap.yaml
kubectl apply -f ./boards-k8s/deploy/deployment.yaml
kubectl apply -f ./boards-k8s/deploy/service.yaml
```

### monitor-K8s-pod
```bash
kubectl apply -f ./monitor-K8s-pod/service-account/service-account.yaml
kubectl apply -f ./monitor-K8s-pod/service-account/role.yaml
kubectl apply -f ./monitor-K8s-pod/service-account/role-binding.yaml
kubectl apply -f ./monitor-K8s-pod/deploy/configmap.yaml
kubectl apply -f ./monitor-K8s-pod/deploy/deployment-rabbitmq.yaml
kubectl apply -f ./monitor-K8s-pod/deploy/service-rabbitmq.yaml
kubectl apply -f ./monitor-K8s-pod/deploy/deployment.yaml
```

## ✅ Verificação e Testes

### Verificar se os pods estão rodando:
```bash
kubectl get pods -A
```

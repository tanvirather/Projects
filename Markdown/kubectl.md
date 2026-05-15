# kubectl commands

## config

```sh
kubectl config current-context # Display the current-context
kubectl config get-contexts # Describe one or many contexts
kubectl config set-context --current --namespace=<namespace>  # Set a context entry in kubeconfig
```

## Namespaces

```sh
kubectl get namespaces # List namespaces
kubectl delete namespace <namespace> # Delete namespace
kubectl create namespace <namespace> --dry-run=client --output yaml > ${namespace}.yaml # Create namespace yaml file
kubectl apply --filename <namespace>.yaml # Apply the file
```

## Pods

```sh
kubectl get pods -A # List all pods in the current namespace
```

## Deployments

```sh
kubectl get deployments # List all deployments
kubectl delete deployment $deployment # delete named pod
kubectl create deployment $deployment --image=$image --dry-run=client --output yaml > ${namespace}.yaml # Create namespace yaml file
kubectl apply --filename ${namespace}.yaml # Apply the file
```

## ingress

```sh
kubectl get ingress # List all deployments
# kubectl delete deployment $deployment # delete named pod
kubectl create ingress $deployment --dry-run=client --output yaml > ingress.yaml # Create namespace yaml file
kubectl apply --filename ${namespace}.yaml # Apply the file
```

## Pods

```sh
kubectl get pods # List all pods in the current namespace
kubectl get pods --namespace $namespace # List all pods in the namespace
kubectl get pods --all-namespaces --output wide # List all pods in all namespaces with wide details
kubectl run $pod --image=$image --dry-run=client --output yaml > ${pod}.yaml # Create pod yaml file
kubectl apply --filename ${pod}.yaml # Apply the file
kubectl delete pod $pod # delete named pod
kubectl exec -it $pod -- /bin/bash # run bash inside the pod
kubectl exec -it $pod c <container_name> -- /bin/bash # run bash inside the pod
```

## service

```sh
kubectl get service
```

## port-forward

```sh
kubectl port-forward pods/${pod} 9000
```

## persistent volumes

```sh
# kubectl get ingress # List all deployments
# # kubectl delete deployment $deployment # delete named pod
# kubectl create ingress $deployment --dry-run=client --output yaml > ingress.yaml # Create namespace yaml file
# kubectl apply --filename ${namespace}.yaml # Apply the file
```

# logs

```

```

# Dictionary

- **CNI**: Container Network Interface

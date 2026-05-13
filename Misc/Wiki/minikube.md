# minikube commands

```sh
minikube delete # Deletes a local Kubernetes cluster
minikube start # Starts a local Kubernetes cluster
minikube addons enable metrics-server # Enable the Metrics Server Add-On
minikube addons enable registry # Enable the Local Registry Add-On
minikube addons list # display addons
minikube tunnel # Creates a network route to services deployed with type LoadBalancer
minikube dashboard --port=39663 # Opens the Kubernetes dashboard in a browser
kubectl port-forward --namespace kube-system svc/registry 5000:80 # port-forward registry service to localhost:5000
```

export class NumericTypeStore {
  #apiClient;
  #url = "numericType"

  constructor(apiClient) {
    this.#apiClient = apiClient
  }

  async get(id) {
    if (id) {
      return await this.#apiClient.get(`${this.#url}/?id=${id}`);
    }
    return await this.#apiClient.get(`${this.#url}`);
  }

  async post(model) {
    return await this.#apiClient.post(this.#url, model);
  }

  async put(model) {
    return await this.#apiClient.put(this.#url, model);
  }

  async delete(id) {
    return await this.#apiClient.delete(`${this.#url}/?id=${id}`);
  }
}

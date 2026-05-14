package model

import (
	"github.com/google/uuid"
)

type Account struct {
	Id    uuid.UUID `json:"id"`
	Email string    `json:"email"`
}

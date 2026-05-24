export const GrupoSanguineo = {
  APositivo: 1,
  ANegativo: 2,
  BPositivo: 3,
  BNegativo: 4,
  ABPositivo: 5,
  ABNegativo: 6,
  OPositivo: 7,
  ONegativo: 8
} as const;

export type GrupoSanguineo =
  (typeof GrupoSanguineo)[keyof typeof GrupoSanguineo];
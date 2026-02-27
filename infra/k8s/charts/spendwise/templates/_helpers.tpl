{{/*
Common labels applied to all resources
*/}}
{{- define "spendwise.labels" -}}
app.kubernetes.io/managed-by: {{ .Release.Service }}
app.kubernetes.io/part-of: spendwise
helm.sh/chart: {{ .Chart.Name }}-{{ .Chart.Version }}
{{- end }}
{{/*
Selector labels for a specific service
*/}}
{{- define "spendwise.selectorLabels" -}}
app.kubernetes.io/name: {{ .name }}
app.kubernetes.io/instance: {{ .release }}
{{- end }}
{{/*
Full image path helper
Usage: {{ include "spendwise.image" (dict "registry" .Values.global.image.registry "repository" .Values.global.image.repository "tag" .tag) }}
*/}}
{{- define "spendwise.image" -}}
{{ .registry }}/{{ .repository }}:{{ .tag }}
{{- end }}
{{/*
Database host — returns internal or external depending on config
*/}}
{{- define "spendwise.dbHost" -}}
{{- if .Values.postgres.enabled -}}
postgres-service
{{- else -}}
{{ .Values.externalDatabase.host }}
{{- end -}}
{{- end }}
{{/*
Database port
*/}}
{{- define "spendwise.dbPort" -}}
{{- if .Values.postgres.enabled -}}
{{ .Values.postgres.port }}
{{- else -}}
{{ .Values.externalDatabase.port }}
{{- end -}}
{{- end }}
{{/*
Database user
*/}}
{{- define "spendwise.dbUser" -}}
{{- if .Values.postgres.enabled -}}
{{ .Values.postgres.user }}
{{- else -}}
{{ .Values.externalDatabase.user }}
{{- end -}}
{{- end }}
{{/*
Connection string builder
Usage: {{ include "spendwise.connectionString" (dict "Values" .Values "dbName" "authservice" "connKey" "AuthDb") }}
*/}}
{{- define "spendwise.connectionString" -}}
Host={{ include "spendwise.dbHost" . }};Port={{ include "spendwise.dbPort" . }};Database={{ .dbName }};Username={{ include "spendwise.dbUser" . }};Password=$(DB_PASSWORD)
{{- end }}

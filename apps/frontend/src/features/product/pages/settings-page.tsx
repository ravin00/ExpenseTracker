import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Skeleton } from '@/components/ui/skeleton'
import { EmptyState, FormField, PageHeader } from '@/components/product'
import { useSettingsQuery } from '@/features/product/data/api'
import { zodResolver } from '@hookform/resolvers/zod'
import { AlertTriangle, Save } from 'lucide-react'
import { useForm } from 'react-hook-form'
import { toast } from 'sonner'
import { z } from 'zod'

const settingsSchema = z.object({
  fullName: z.string().min(2, 'Please enter your full name'),
  email: z.string().email('Please enter a valid email address'),
  timezone: z.string().min(2, 'Timezone is required'),
  currency: z.string().length(3, 'Currency code must be 3 letters'),
  weeklyDigest: z.boolean(),
  budgetAlerts: z.boolean(),
  marketingEmails: z.boolean(),
})

type SettingsFormData = z.infer<typeof settingsSchema>

export function SettingsPage() {
  const { data, isLoading, isError } = useSettingsQuery()

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<SettingsFormData>({
    resolver: zodResolver(settingsSchema),
    values: data
      ? {
          fullName: data.profile.fullName,
          email: data.profile.email,
          timezone: data.profile.timezone,
          currency: data.profile.currency,
          weeklyDigest: data.preferences.weeklyDigest,
          budgetAlerts: data.preferences.budgetAlerts,
          marketingEmails: data.preferences.marketingEmails,
        }
      : undefined,
  })

  const onSubmit = async (values: SettingsFormData) => {
    await new Promise((resolve) => setTimeout(resolve, 400))
    toast.success(`Settings saved for ${values.fullName}`)
  }

  if (isLoading) {
    return (
      <div className="space-y-4">
        <Skeleton className="h-20 w-full" />
        <Skeleton className="h-[420px] w-full" />
      </div>
    )
  }

  if (isError || !data) {
    return (
      <EmptyState
        icon={AlertTriangle}
        title="Settings unavailable"
        description="Unable to load settings. Confirm API connectivity and retry."
      />
    )
  }

  return (
    <div className="space-y-6">
      <PageHeader
        title="Settings"
        description="Manage profile details, communication preferences, and regional defaults."
      />

      <form onSubmit={handleSubmit(onSubmit)} className="surface grid gap-5 rounded-2xl p-5 sm:grid-cols-2">
        <FormField id="fullName" label="Full name" error={errors.fullName?.message}>
          <Input id="fullName" {...register('fullName')} />
        </FormField>

        <FormField id="email" label="Email" error={errors.email?.message}>
          <Input id="email" type="email" {...register('email')} />
        </FormField>

        <FormField id="timezone" label="Timezone" error={errors.timezone?.message}>
          <Input id="timezone" {...register('timezone')} />
        </FormField>

        <FormField id="currency" label="Currency" hint="ISO code (e.g. USD)" error={errors.currency?.message}>
          <Input id="currency" maxLength={3} {...register('currency')} />
        </FormField>

        <div className="sm:col-span-2">
          <h2 className="text-sm font-semibold text-foreground">Notifications</h2>
          <div className="mt-3 space-y-3">
            <label className="flex items-center gap-3 rounded-xl border border-border/80 bg-background/70 p-3">
              <input
                type="checkbox"
                {...register('weeklyDigest')}
                className="focus-ring h-4 w-4 rounded border-border"
                aria-label="Enable weekly digest"
              />
              <div>
                <p className="text-sm font-medium">Weekly digest</p>
                <p className="text-xs text-muted-foreground">Summary email every Monday.</p>
              </div>
            </label>

            <label className="flex items-center gap-3 rounded-xl border border-border/80 bg-background/70 p-3">
              <input
                type="checkbox"
                {...register('budgetAlerts')}
                className="focus-ring h-4 w-4 rounded border-border"
                aria-label="Enable budget alerts"
              />
              <div>
                <p className="text-sm font-medium">Budget alerts</p>
                <p className="text-xs text-muted-foreground">Immediate warning when any budget is near limit.</p>
              </div>
            </label>

            <label className="flex items-center gap-3 rounded-xl border border-border/80 bg-background/70 p-3">
              <input
                type="checkbox"
                {...register('marketingEmails')}
                className="focus-ring h-4 w-4 rounded border-border"
                aria-label="Enable product updates"
              />
              <div>
                <p className="text-sm font-medium">Product updates</p>
                <p className="text-xs text-muted-foreground">Occasional announcements and release notes.</p>
              </div>
            </label>
          </div>
        </div>

        <div className="sm:col-span-2 flex justify-end">
          <Button type="submit" disabled={isSubmitting}>
            <Save className="h-4 w-4" />
            Save changes
          </Button>
        </div>
      </form>
    </div>
  )
}
